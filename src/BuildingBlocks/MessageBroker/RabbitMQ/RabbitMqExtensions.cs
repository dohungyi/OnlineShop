using MassTransit;
using MassTransit.RabbitMqTransport;
using MessageBroker.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.RabbitMQ;

public static class RabbitMqExtensions
{
    public static IServiceCollection ConfigureMasstransitRabbitMQ(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator, MessageQueueSettings> registerConsumer = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator, MessageQueueSettings> configConsumer = null)
    {

        var messageQueueSettings = configuration.GetSection(MessageQueueSettings.SectionName)
            .Get<MessageQueueSettings>();
        
        if (messageQueueSettings is null)
        {
            throw new ArgumentNullException(nameof(MessageQueueSettings));
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
}