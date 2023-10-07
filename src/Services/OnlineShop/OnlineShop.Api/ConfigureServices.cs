using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
        
        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            x.MultipartHeadersLengthLimit = int.MaxValue;
        });
        
        //services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

        //services.AddResponseCompression(options =>
        //{
        //    options.EnableForHttps = true;
        //    options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
        //    options.Providers.Add<GzipCompressionProvider>();
        //});

        services.AddCors();

        services.AddCoreLocalization();

        services.AddCoreRateLimit();

        services.AddCoreBehaviors();
        
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