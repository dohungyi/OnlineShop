using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel.Application;
using SharedKernel.Domain;

namespace SharedKernel.Persistence;

public abstract class AppDbContext : DbContext, IAppDbContext
{
    #region [CONSTRUCTOR]

    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    #endregion [CONSTRUCTOR]
    
    #region [EVENTS]
    
    private List<DomainEvent> DomainEvents { get; set; } = new();
    
    public async Task PublishEvents(IEventDispatcher eventDispatcher, CancellationToken cancellationToken)
    {
        await Task.Yield();
        if (DomainEvents is not null && DomainEvents.Any())
        {
            var events = DomainEvents.Select(x => x).ToList();
            _ = eventDispatcher.PublishEvent(events, cancellationToken);

            DomainEvents.Clear();
        }
    }
    
    #endregion

    #region [METHOD]
    public virtual async Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
    {
        if (entities is not null && typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            foreach (var entity in entities)
            {
                var @base = (IBaseEntity)entity;
                if (@base.DomainEvents is not null && @base.DomainEvents.Any())
                {
                    DomainEvents.AddRange(@base.DomainEvents);
                }
            }
        }

        if (typeof(IBaseEntity).IsAssignableFrom(typeof(IAuditable)))
        {
            await this.BulkUpdateAsync(entities, cancellationToken: cancellationToken);
        }
        else
        {
            await this.BulkDeleteAsync(entities, cancellationToken: cancellationToken);
        }
    }
    
    public virtual async Task BulkInsertEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
    {
        if (entities is not null && typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            foreach (var entity in entities)
            {
                var @base = (IBaseEntity)entity;
                if (@base.DomainEvents is not null && @base.DomainEvents.Any())
                {
                    DomainEvents.AddRange(@base.DomainEvents);
                }
            }
        }
        
        await this.BulkInsertAsync(entities, cancellationToken: cancellationToken);
       
    }

    public virtual async Task BulkCommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        await this.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        
        if (!dispatchEvent)
        {
            DomainEvents.Clear();
        }
    }
        
    #endregion

    #region [IMPLEMENT IUNITOFWORK]
    
    public virtual async Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    { 
        if (dispatchEvent)
        {
            ProcessDomainEvents();
        }
        
        await SaveChangesAsync(cancellationToken);
    }
    
    #endregion
    
    protected virtual void ProcessDomainEvents()
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<IBaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents is not null && e.DomainEvents.Any())
            .ToList();

        if (entitiesWithEvents.Any())
        {
            DomainEvents = entitiesWithEvents
                .SelectMany(e => e.DomainEvents)
                .ToList();
        }
    }
    
}