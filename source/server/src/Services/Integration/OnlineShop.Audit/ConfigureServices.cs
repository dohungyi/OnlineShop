using MassTransit;
using SharedKernel.Configure;

namespace OnlineShop.Audit;

public static class ConfigureServices
{
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
                cfg.ConfigureEndpoints(context);
            }
        );
        
        services.AddMassTransitHostedService();
        
        return services;
    }
}