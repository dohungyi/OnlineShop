using System.IO.Compression;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Configure;
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
        
        #endregion
        
        return services;
    }

    public static IServiceCollection AddCoreMasstransitRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCoreRabbitMq(
            configuration,
            (configurator, setting) =>
            {
                // configurator.AddConsumer<CrawlerDataConsumer>();
                //
                // configurator.AddConsumer<PushNotificationGroupConsumer>();
                //
                // configurator.AddRequestClient<CrawlerDataConsumer>(new Uri(
                //     setting.GetPublishEndpoint("crawler-data")));
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
                
                cfg.ConfigureEndpoints(context);
                
            }
        );
        
        services.AddMassTransitHostedService();
        
        return services;
    }

}