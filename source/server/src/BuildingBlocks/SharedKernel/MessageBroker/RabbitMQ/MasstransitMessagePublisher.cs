using MassTransit;

namespace SharedKernel.MessageBroker;

public class MasstransitMessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publish;

    public MasstransitMessagePublisher(IPublishEndpoint publish)
    {
        _publish = publish;
    }
    
    /// <summary>
    /// Đẩy một message lên message queue và metaData nếu có
    /// </summary>
    public async Task PublishAsync<T>(T @event, Dictionary<string, string>? metaData = null, CancellationToken cancellationToken = default) where T : class
    {
        await _publish.Publish(@event, ctx =>
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