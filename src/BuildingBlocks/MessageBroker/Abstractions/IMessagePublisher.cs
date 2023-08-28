namespace MessageBroker.Abstractions;

public interface IMessagePublisher
{
    Task Publish<T>(
        T message,
        Dictionary<string, string>? metaData = null,
        CancellationToken cancellationToken = default) where T : class;
}