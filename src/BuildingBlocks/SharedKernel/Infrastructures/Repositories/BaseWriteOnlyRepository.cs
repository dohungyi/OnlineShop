using EFCore.BulkExtensions;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Application.Consts;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.Libraries.Utility;
using Resources = SharedKernel.Properties.Resources;


namespace SharedKernel.Infrastructures.Repositories;

public class BaseWriteOnlyRepository<TEntity, TDbContext> : IBaseWriteOnlyRepository<TEntity, TDbContext>
    where TEntity :  BaseEntity
    where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly string _tableName;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly ICurrentUser _currentUser;
    protected readonly IStringLocalizer<Resources> _localizer;
    
    public BaseWriteOnlyRepository(TDbContext dbContext, ISequenceCaching sequenceCaching, ICurrentUser currentUser, IStringLocalizer<Resources> localizer)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _sequenceCaching = sequenceCaching ?? throw new ArgumentNullException(nameof(sequenceCaching));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        
        _dbSet = dbContext.Set<TEntity>();
        _tableName = nameof(TEntity);
    }

    #region [EVENT]
    
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

    #region [INSERT]
    
    public void Insert(TEntity entity)
    {
        _dbContext.Add(entity);
    }

    public void Insert(IList<TEntity> entities)
    {
        _dbContext.AddRange(entities);
    }

    public void BulkInsert(IList<TEntity> listEntities)
    {
        _dbContext.BulkInsert(listEntities);
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public async Task BulkInsertAsync(IList<TEntity> listEntities, CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkInsertAsync(listEntities, cancellationToken: cancellationToken);
    }
    
    #endregion

    #region [UPDATE]
    
    public void Update(TEntity entity)
    {
        _dbContext.Update(entity);
    }

    public void Update(IList<TEntity> entities)
    {
        _dbContext.UpdateRange(entities);
    }

    public void BulkUpdate(IList<TEntity> listEntities)
    {
        _dbContext.BulkUpdate(listEntities);
    }

    public async Task BulkUpdateAsync(IList<TEntity> listEntities, CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkUpdateAsync(listEntities, cancellationToken: cancellationToken);
    }
    
    #endregion

    #region [DELETE]

    public void Delete(TEntity entity)
    {
        _dbContext.Remove(entity);
    }

    public void Delete(IList<TEntity> entities)
    {
        _dbContext.RemoveRange(entities);
    }

    public void BulkDelete(IList<TEntity> listEntities)
    {
        _dbContext.BulkDelete(listEntities);
    }

    public async Task BulkDeleteAsync(IList<TEntity> listEntities, CancellationToken cancellationToken = default)
    {
        if (listEntities is null && listEntities.Any())
        {
            BeforeDelete(listEntities);
            await _dbContext.BulkDeleteAsync(listEntities, cancellationToken: cancellationToken);
        }
    }

    #endregion

    #region [PROTECTED]
    protected virtual void BeforeInsert(IEnumerable<TEntity> entities)
    {
        var batches = entities.ChunkList(1000);
        batches.ToList().ForEach(async entities =>
        {
            entities.ForEach(entity =>
            {
                var clone = (TEntity)entity.Clone();
                clone.ClearDomainEvents();

                entity.AddDomainEvent(new InsertAuditEvent<TEntity>(new List<TEntity> { clone }, _currentUser));
            });

            if (batches.Count() > 1)
            {
                await Task.Delay(69);
            }
        });
        
    }

    protected virtual void BeforeUpdate(TEntity entity, TEntity oldValue)
    {
       
        var newValue = (TEntity)entity.Clone();
        newValue.ClearDomainEvents();
        oldValue.ClearDomainEvents();

        entity.AddDomainEvent(new UpdateAuditEvent<TEntity>(new List<UpdateAuditModel<TEntity>> { new UpdateAuditModel<TEntity>(newValue, oldValue) }, _currentUser));
    }

    protected virtual void BeforeDelete(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            var clone = (TEntity)entity.Clone();
            clone.ClearDomainEvents();
            entity.AddDomainEvent(new DeleteAuditEvent<TEntity>(new List<TEntity> { clone }, _currentUser));
        }
    }
    protected virtual async Task ClearCacheWhenChangesAsync(List<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        var fullRecordKey = BaseCacheKeys.GetSystemFullRecordsKey(nameof(TEntity));
        tasks.Add(_sequenceCaching.RemoveAsync(fullRecordKey, cancellationToken: cancellationToken));

        if (ids is not null && ids.Any())
        {
            foreach (var id in ids)
            {
                var recordByIdKey = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id);
                tasks.Add(_sequenceCaching.RemoveAsync(recordByIdKey, cancellationToken: cancellationToken));
            }
        }
    }
    #endregion
    
}