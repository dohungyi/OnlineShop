namespace SharedKernel.MessageBroker;

public interface IMessagePublisher
{
    Task PublishAsync<T>(
        T @event,
        Dictionary<string, string>? metaData = null,
        CancellationToken cancellationToken = default) where T : class;
}