using MassTransit;
using SharedKernel.Configure;

namespace OnlineShop.Integrations;

public static class ConfigureServices
{
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