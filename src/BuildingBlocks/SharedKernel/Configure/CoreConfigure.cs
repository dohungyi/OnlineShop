using System.Globalization;
using System.Net;
using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using SharedKernel.Application.Responses;
using SharedKernel.Core;
using SharedKernel.Filters;
using SharedKernel.Libraries.Security;
using SharedKernel.Log;
using SharedKernel.Middlewares;
using SharedKernel.Properties;
using SharedKernel.Runtime.Exceptions;
using SharedKernel.SignalR;

namespace SharedKernel.Configure;

public static class CoreConfigure
{
    #region [DEPENDENCY INJECTION]

    

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
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120),
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<MessageHub>("/socket-message");
        });
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
    
    #endregion
}