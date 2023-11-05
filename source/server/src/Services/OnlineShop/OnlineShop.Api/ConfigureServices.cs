using System.IO.Compression;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OnlineShop.Api.ControllerFilters;
using OnlineShop.Domain.Events.Auth;
using SharedKernel.Configure;
using SharedKernel.MessageBroker;
using ZymLabs.NSwag.FluentValidation;

namespace OnlineShop.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ => configuration);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Điều này đăng ký dịch vụ xác thực chứng chỉ vào hệ thống xác thực của ứng dụng
        services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
        
        // Đoạn mã này đang cấu hình các tùy chọn cho xử lý dữ liệu form.
        // Cụ thể, nó đang cấu hình giới hạn độ dài giá trị, độ dài của phần thân và phần đầu của yêu cầu dữ liệu đa phần (multipart)
        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            x.MultipartHeadersLengthLimit = int.MaxValue;
        });
        
        // Đoạn mã đang cấu hình mức độ nén cho việc nén gzip. Trong trường hợp này, nó được cấu hình để sử dụng mức độ nén nhanh nhất 
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

        // cấu hình này giúp ứng dụng của bạn có khả năng nén phản hồi để giảm băng thông và tăng tốc độ truyền tải cho client.
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.AddCors();

        services.AddCoreLocalization();

        services.AddCoreRateLimit();
        
        #region AddController + CamelCase + FluentValidation

        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        
        services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // Api Versioning
        services.AddApiVersioning(configuration);
        
        #endregion
        
        return services;
    }

    public static IServiceCollection AddApiVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
        });
        
        services.AddVersionedApiExplorer(
            options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineShop.Api version one", Version = "v1" });
            
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "OnlineShop.Api version two", Version = "v2" });
            
            c.DocumentFilter<HideOcelotControllersFilter>();
            
            // Configure Swagger to use the JWT bearer authentication scheme
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securityScheme);
    
            // Make Swagger require a JWT token to access the endpoints
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    securityScheme,
                    new string[] {}
                }
            });
        });

        return services;
    }
    
    public static IServiceCollection AddCoreMasstransitRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCoreRabbitMq(
            configuration,
            (configurator, setting) =>
            {
                
            },
            (context, cfg, setting) =>
            {
                // cfg.ReceiveEndpoint($"{setting.GetReceiveEndpoint(NotificationTopic.TopicConstant.PushNotificationGroup)}",
                //     endpointConfigurator =>
                //     {
                //         endpointConfigurator.Bind($"{NotificationTopic.TopicConstant.PushNotificationGroup}");
                //         endpointConfigurator.Bind<PushNotificationGroupMessage>();
                //         endpointConfigurator.UseRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
                //         endpointConfigurator.UseRateLimit(5);
                //         endpointConfigurator.ConfigureConsumer<PushNotificationGroupConsumer>(context);
                //     });
                //

                cfg.ReceiveEndpoint($"{setting.GetReceiveEndpoint("audit-event")}",
                    endpointConfigurator =>
                {
                    endpointConfigurator.Bind("audit-event");
                    endpointConfigurator.Bind<SignInAuditEvent>();
                });

                
                cfg.ConfigureEndpoints(context);
                
            }
        );
        
        services.AddMassTransitHostedService();
        
        return services;
    }

}