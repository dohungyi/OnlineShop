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
    
    public TEntity Insert(TEntity entity)
    {
        BeforeInsert(new List<TEntity>() { entity });
        
        _dbContext.Add(entity);

        return entity;
    }

    public IList<TEntity> Insert(IList<TEntity> entities)
    {
        BeforeInsert(entities);
        
        _dbContext.AddRange(entities);

        return entities;
    }

    public IList<TEntity> BulkInsert(IList<TEntity> entities)
    {
        BeforeInsert(entities);
        
        _dbContext.BulkInsert(entities);

        return entities;
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeInsert(new List<TEntity>(){ entity });
        
        await _dbSet.AddAsync(entity, cancellationToken);
        
        return entity;
    }

    public async Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entities);
        
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        
        return entities;
    }

    public async Task<IList<TEntity>> BulkInsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entities);
        
        await _dbContext.BulkInsertAsync(entities, cancellationToken: cancellationToken);
        
        return entities;
    }
    
    #endregion

    #region [UPDATE]
    
    public void Update(TEntity entity)
    {
        var currentEntity = _dbSet.Find(entity.Id);
        
        BeforeUpdate(entity, currentEntity);
        
        _dbContext.Entry(entity).State = EntityState.Modified;
    }
    
    #endregion

    #region [DELETE]

    public void Delete(TEntity entity)
    {
        BeforeDelete(new List<TEntity>() { entity });
        
        _dbContext.Remove(entity);
    }

    public void Delete(IList<TEntity> entities)
    {
        BeforeDelete(entities);
        
        _dbContext.RemoveRange(entities);
    }

    public void BulkDelete(IList<TEntity> entities)
    {
        BeforeDelete(entities);
        
        _dbContext.BulkDelete(entities);
    }

    public async Task BulkDeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeDelete(entities);
        
        await _dbContext.BulkDeleteAsync(entities, cancellationToken: cancellationToken);
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
                entity.Id = Guid.NewGuid();
                entity.CreatedBy = _currentUser.Context.Username;
                entity.CreatedDate = DateHelper.Now;
                entity.LastModifiedDate = null;
                entity.LastModifiedBy = null;
                entity.DeletedDate = null;
                entity.DeletedBy = null;
                
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
        entity.LastModifiedDate = DateHelper.Now;
        entity.LastModifiedBy = _currentUser.Context.Username;
        entity.DeletedDate = null;
        entity.DeletedBy = null;
        
        var newValue = (TEntity)entity.Clone();
        newValue.ClearDomainEvents();
        oldValue.ClearDomainEvents();

        entity.AddDomainEvent(new UpdateAuditEvent<TEntity>(new List<UpdateAuditModel<TEntity>> { new UpdateAuditModel<TEntity>(newValue, oldValue) }, _currentUser));
    }

    protected virtual void BeforeDelete(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.DeletedDate = DateHelper.Now;
            entity.DeletedBy = _currentUser.Context.Username;
            
            var clone = (TEntity)entity.Clone();
            clone.ClearDomainEvents();
            
            entity.AddDomainEvent(new DeleteAuditEvent<TEntity>(new List<TEntity> { clone }, _currentUser));
        }
    }
    protected virtual async Task ClearCacheWhenChangesAsync(List<Guid> ids, CancellationToken cancellationToken = default)
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