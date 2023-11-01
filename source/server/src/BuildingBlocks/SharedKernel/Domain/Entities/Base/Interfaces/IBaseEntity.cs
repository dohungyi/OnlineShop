namespace SharedKernel.Domain;

public interface IBaseEntity<TKey> : IAuditable, ICoreEntity, ICloneable
{
    TKey Id { get; set; }
    bool IsDeleted { get; set; }
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void AddDomainEvent(DomainEvent @event);

    void RemoveDomainEvent(DomainEvent @event);

    void ClearDomainEvents();
}

public interface IBaseEntity : IBaseEntity<Guid> { }