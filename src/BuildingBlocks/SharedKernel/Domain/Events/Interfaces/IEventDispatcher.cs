namespace SharedKernel.Domain;

public interface IEventDispatcher
{
    Task FireEvent<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent;

    Task FireEvent<T>(List<T> events, CancellationToken cancellationToken = default) where T : DomainEvent;
}