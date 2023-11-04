using System.Globalization;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using AspNetCoreRateLimit;
using FluentValidation;
using HealthChecks.UI.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using SharedKernel.Application;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Core;
using SharedKernel.Domain;
using SharedKernel.Domain.DomainEvents.Dispatcher;
using SharedKernel.Filters;
using SharedKernel.Infrastructures;
using SharedKernel.Libraries.Security;
using SharedKernel.Log;
using SharedKernel.MessageBroker;
using SharedKernel.Middlewares;
using SharedKernel.Persistence.ExceptionHandler;
using SharedKernel.Properties;
using SharedKernel.Providers;
using SharedKernel.Runtime.Exceptions;
using StackExchange.Redis;

namespace SharedKernel.Configure;

public static class CoreConfigure
{
    #region [DEPENDENCY INJECTION]
    
    public static IServiceCollection AddCoreLocalization(this IServiceCollection services)
    {
        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() };
        });

        return services;
    }
    
    public static IServiceCollection AddCoreRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator, RabbitMqSetting> registerConsumer = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator, RabbitMqSetting> configConsumer = null)
    {
        var messageQueueSettings = configuration.GetSection(RabbitMqSetting.SectionName)
            .Get<RabbitMqSetting>();

        if (messageQueueSettings is null)
        {
            throw new ArgumentNullException(nameof(RabbitMqSetting));
        }

        services.AddMassTransit(configurator =>
        {
            registerConsumer?.Invoke(configurator, messageQueueSettings);

            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(messageQueueSettings.Host, messageQueueSettings.Port, messageQueueSettings.VirtualHost, h =>
                {
                    h.Username(messageQueueSettings.UserName);
                    h.Password(messageQueueSettings.Password);
                });

                configConsumer?.Invoke(context, cfg, messageQueueSettings);
            });
        });

        services.AddTransient<IMessagePublisher, MasstransitMessagePublisher>();

        return services;
    }
        

    public static IServiceCollection AddCoreAuthentication(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        
        }).AddJwtBearer(jwtOptions =>
        {
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = DefaultJwtConfig.Audience,
                ValidateIssuer = true,
                ValidIssuer = DefaultJwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DefaultJwtConfig.Key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        
            jwtOptions.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
        
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/socket-message"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        // }).AddGoogle(googleOptions =>
        // {
        //     googleOptions.ClientId = DefaultGoogleConfig.ClientId;
        //     googleOptions.ClientSecret = DefaultGoogleConfig.ClientSecret;
        });
        return services;
    }
    
    public static IServiceCollection AddCoreCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCacheSettings = configuration.GetSection(RedisCacheSettings.SectionName).Get<RedisCacheSettings>();
        var asyncPolicy = PollyExtensions.CreateDefaultPolicy(cfg =>
        {
            cfg.Or<RedisServerException>()
                .Or<RedisConnectionException>();
        });
        
        services.AddSingleton<IRedisCache>(s => new RedisCache(
            redisCacheSettings.ConnectionString,
            redisCacheSettings.InstanceName, 
            redisCacheSettings.DatabaseIndex,
            asyncPolicy));


        services.AddMemoryCache();
        services.AddSingleton<IMemoryCaching>(s =>
            new MemoryCaching(s.GetRequiredService<IMemoryCache>()));
        
        services.AddSingleton<ISequenceCaching, SequenceCaching>();
        
        return services;
    }
    
    public static IServiceCollection AddCoreProviders(this IServiceCollection services)
    {
        services.AddScoped<IS3StorageProvider, S3StorageProvider>();
        return services;
    }
    
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
    
    public static IServiceCollection AddBus(this IServiceCollection services)
    {
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        return services;
    }
    
    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();
        return services;
    }
    
    public static IServiceCollection AddCoreRateLimit(this IServiceCollection services)
    {
        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.RealIpHeader = HeaderNamesExtension.RealIpHeader;
            options.ClientIdHeader = HeaderNamesExtension.ClientIdHeader;
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1s",
                    Limit = 8,
                }
            };
        });

        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();

        return services;
    }
    
    public static IServiceCollection AddCoreBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        return services;
    }

    #endregion [DEPENDENCY INJECTION]

    #region [MIDDLEWARES]

    public static void UseCoreConfigure(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseCoreLocalization();
        if (!environment.IsDevelopment())
        {
            app.UseReject3P();
        }
        app.UseCoreExceptionHandler();
        app.UseIpRateLimiting();
        app.UseForwardedHeaders();
        app.UseHttpsRedirection();
        app.UseCoreUnauthorized();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        // app.UseWebSockets(new WebSocketOptions
        // {
        //     KeepAliveInterval = TimeSpan.FromSeconds(120),
        // });

        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapHub<MessageHub>("/socket-message");
        // });
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        app.UseCoreHealthChecks();
    }
    
    public static void UseCoreLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
    }
    
    public static void UseCoreHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = (HealthCheckRegistration _) => true,
            ResponseWriter = new Func<HttpContext, HealthReport, Task>(UIResponseWriter.WriteHealthCheckUIResponse)
        });
    }

    public static void UseCoreSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
            c.RoutePrefix = string.Empty;
        });
    }

    public static void UseCoreExceptionHandler(this IApplicationBuilder app)
    {
        // Handle exception
        app.UseExceptionHandler(a => a.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature.Error;
            var responseContent = new ApiResult();
            var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();

            // Handle Exception
            // Catchable
            if (exception is CatchableException)
            {
                responseContent.Error = new Error(HttpStatusCode.InternalServerError, exception.Message);
                Logging.Error(exception);
            }
            // Forbidden
            else if (exception is ForbiddenException)
            {
                responseContent.Error = new Error(HttpStatusCode.Forbidden, localizer["not_permission"].Value);
            }
            // Bad request
            else if (exception is BadRequestException badRequestException)
            {
                if (badRequestException.Body is not null)
                {
                    responseContent = new ApiSimpleResult()
                    {
                        Data = badRequestException.Body,
                        Error = new Error(HttpStatusCode.BadRequest, exception.Message, (exception as BadRequestException).Type)
                    };
                }
                else
                {
                    responseContent.Error = new Error(HttpStatusCode.BadRequest, exception.Message, badRequestException.Type);
                }
            }
            // Fluent validation
            else if(exception is ValidationException validationException)
            {
                string errors = string.Join(", ", validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => string.Join("; ", x.Select(y => y.ErrorMessage))
                    )
                    .Select(kv => $"{kv.Key}: {kv.Value}"));
                
                responseContent.Error = new Error(HttpStatusCode.BadRequest, errors, "BAD_REQUEST");
            }
            // Unknown exception
            else
            {
                responseContent.Error = new Error(HttpStatusCode.InternalServerError, localizer["system_error_occurred"].Value);
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseContent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
            
        }));
    }

    public static void UseCoreCors(this IApplicationBuilder app, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredSection("Allowedhosts").Value;
        if (origins.Equals("*"))
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }
        else
        {
            app.UseCors(x => x.WithOrigins(origins.Split(";")).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
        }
    }

    #endregion

    #region [ELK]

    public static IHostBuilder UseCoreSerilog(this IHostBuilder builder) => builder.UseSerilog((context, loggerConfiguration) =>
    {
        CoreSettings.SetDefaultElasticSearchConfig(context.Configuration);
        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Application", DefaultElasticSearchConfig.ApplicationName)
            .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
            .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(DefaultElasticSearchConfig.Uri))
            {
                IndexFormat = $"{DefaultElasticSearchConfig.ApplicationName}-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                ModifyConnectionSettings = x => x.BasicAuthentication(DefaultElasticSearchConfig.Username, DefaultElasticSearchConfig.Password)
            })
            .ReadFrom.Configuration(context.Configuration);
    })
    .ConfigureServices(services =>
    {
        var sp = services.BuildServiceProvider();
        var logger = sp.GetRequiredService<ILogger>();
        var configuration = sp.GetRequiredService<IConfiguration>();

        CoreSettings.SetLoggingConfig(configuration, logger);
    });
    
    [Obsolete]
    public static IWebHostBuilder UseCoreSerilog(this IWebHostBuilder builder) =>
    builder.UseSerilog((context, loggerConfiguration) =>
    {
        CoreSettings.SetDefaultElasticSearchConfig(context.Configuration);
        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Application", DefaultElasticSearchConfig.ApplicationName)
            .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
            .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(DefaultElasticSearchConfig.Uri))
            {
                IndexFormat = $"{DefaultElasticSearchConfig.ApplicationName}-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                ModifyConnectionSettings = x => x.BasicAuthentication(DefaultElasticSearchConfig.Username, DefaultElasticSearchConfig.Password)
            })
            .ReadFrom.Configuration(context.Configuration);
    })
    .ConfigureServices(services =>
    {
        var sp = services.BuildServiceProvider();
        var logger = sp.GetRequiredService<ILogger>();
        var configuration = sp.GetRequiredService<IConfiguration>();

        CoreSettings.SetLoggingConfig(configuration, logger);
    });
    
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            var applicationName = context.HostingEnvironment.ApplicationName.ToLower().Replace(".", "-");
            var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

            configuration
                .WriteTo.Debug()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp: HH:mm:ss} {Level}] {SourceContext} {NewLine} {Message:lj}{NewLine}{Exception}{NewLine}")
                .WriteTo.File(
                    $@"Logs/{applicationName}-{environmentName}.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
                    fileSizeLimitBytes: 10485760, // 10 MB
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 30
                )
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        };
    
    #endregion
}