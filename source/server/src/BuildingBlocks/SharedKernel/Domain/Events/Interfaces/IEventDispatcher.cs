namespace SharedKernel.Domain;

public interface IEventDispatcher
{
    Task PublishEvent<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent;

    Task PublishEvent<T>(List<T> events, CancellationToken cancellationToken = default) where T : DomainEvent;
}