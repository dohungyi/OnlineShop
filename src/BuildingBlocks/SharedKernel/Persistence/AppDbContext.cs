using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel.Application;
using SharedKernel.Domain;

namespace SharedKernel.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    
    #region [CONSTRUCTOR]

    public AppDbContext() : base()
    {
        
    }
    
    public AppDbContext(string connectionString) : base(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options)
    {
        // Các cài đặt khác nếu cần
    }

    #endregion [CONSTRUCTOR]
    
    #region [EVENTS]
    
    private List<DomainEvent> DomainEvents { get; set; } = new();
    
    public async Task PublishEvents(IEventBus eventBus, CancellationToken cancellationToken)
    {
        await Task.Yield();
        if (DomainEvents is not null && DomainEvents.Any())
        {
            var events = DomainEvents.Select(x => x).ToList();
            _ = eventBus.PublishEvent(events, cancellationToken);

            DomainEvents.Clear();
        }
    }
    
    #endregion
    
    #region [OVERIDE]

    public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
    {
        if (typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var @base = (IBaseEntity)entity;
            if (@base.DomainEvents is not null && @base.DomainEvents.Any())
            {
                DomainEvents.AddRange(@base.DomainEvents);
            }
        }
        
        return base.AddAsync(entity, cancellationToken);
    }
    public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = new CancellationToken())
    {
        if (entities is not null && typeof(IBaseEntity).IsAssignableFrom(entities.First()?.GetType()))
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
        return base.AddRangeAsync(entities, cancellationToken);
    }
    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
    {
        if (typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var @base = (IBaseEntity)entity;
            if (@base.DomainEvents is not null && @base.DomainEvents.Any())
            {
                DomainEvents.AddRange(@base.DomainEvents);
            }
        }
        
        return base.Update(entity);
    }
    public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
    {
        if (typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var @base = (IBaseEntity)entity;
            if (@base.DomainEvents is not null && @base.DomainEvents.Any())
            {
                DomainEvents.AddRange(@base.DomainEvents);
            }
        }
        
        return base.Remove(entity);
    }
    public override void RemoveRange(IEnumerable<object> entities)
    {
        
        if (entities is not null && typeof(IBaseEntity).IsAssignableFrom(entities.First()?.GetType()))
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
        
        base.RemoveRange(entities);
    }
    
    #endregion

    #region [METHOD]
    public async Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
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
    
    public async Task BulkInsertEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
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

    public async Task BulkCommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        await this.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        
        if (!dispatchEvent)
        {
            DomainEvents.Clear();
        }
    }
        
    #endregion

    #region [IMPLEMENT IUNITOFWORK]
    
    public async Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        
        if (!dispatchEvent)
        {
            DomainEvents.Clear();
        }
    }
    
    #endregion
    
}