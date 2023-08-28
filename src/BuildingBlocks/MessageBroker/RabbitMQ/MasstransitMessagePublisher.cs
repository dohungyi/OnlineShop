using MassTransit;
using MessageBroker.Abstractions;

namespace MessageBroker.RabbitMQ;

public class MasstransitMessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publish;

    public MasstransitMessagePublisher(IPublishEndpoint publish)
    {
        _publish = publish ?? throw new ArgumentNullException(nameof(publish));
    }
    
    public async Task Publish<T>(T message, Dictionary<string, string>? metaData = null, CancellationToken cancellationToken = default) where T : class
    {
        await _publish.Publish(message, ctx =>
        {
            if (metaData?.Any() ?? false)
            {
                foreach (var hKey in metaData.Keys)
                {
                    ctx.Headers.Set(hKey, metaData[hKey], false);
                }
            }
        }, cancellationToken);
    }
}