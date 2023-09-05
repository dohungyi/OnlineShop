using SharedKernel.Domain.Events.BaseEvents;

namespace SharedKernel.Domain.Entities.Base.Interfaces;

public interface IBaseEntity<TKey> : ICoreEntity, IAuditable, ICloneable
{
    TKey Id { get; set; }
    bool IsDeleted { get; set; }
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void AddDomainEvent(DomainEvent @event);

    void RemoveDomainEvent(DomainEvent @event);

    void ClearDomainEvents();
}

public interface IBaseEntity : IBaseEntity<Guid> { }