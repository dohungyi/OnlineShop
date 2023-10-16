using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;
using SharedKernel.Application.Consts;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.Persistence;

namespace SharedKernel.Infrastructures.Repositories;

public class BaseWriteOnlyRepository<TEntity,TDbContext> : IBaseWriteOnlyRepository<TEntity, TDbContext>
    where TEntity : BaseEntity
    where TDbContext : AppDbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly string _tableName;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly ICurrentUser _currentUser;
    
    public BaseWriteOnlyRepository(
        TDbContext dbContext, 
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _sequenceCaching = sequenceCaching ?? throw new ArgumentNullException(nameof(sequenceCaching));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        
        _dbSet = dbContext.Set<TEntity>();
        _tableName = nameof(TEntity);
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    #region [INSERTS]
    
    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeInsert(new List<TEntity>() {entity});
        
        await _dbContext.AddAsync<TEntity>(entity, cancellationToken);
        
        return entity;
    }

    public async Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entities);
        
        await _dbContext.AddRangeAsync(entities, cancellationToken);
        
        return entities;
    }

    public async Task<IList<TEntity>> BulkInsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entities);
        
        await _dbContext.BulkInsertEntitiesAsync(entities, cancellationToken: cancellationToken);
        
        return entities;
    }

    #endregion

    #region [UPDATE]
    
    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var currentEntity = await _dbSet.FindAsync(entity.Id);
        
        BeforeUpdate(entity, currentEntity);
        
        _dbContext.Update(entity);
        
        await ClearCacheWhenChangesAsync(new List<object>() { entity.Id }, cancellationToken);
        
        return entity;
    }
    
    #endregion

    #region [DELETE]

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeDelete(new List<TEntity>() { entity });
        
        _dbContext.Remove(entity);
        
        await ClearCacheWhenChangesAsync(new List<object>() { entity.Id }, cancellationToken);
    }
    
    public async Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeDelete(entities);
        
        _dbContext.RemoveRange(entities);

        await ClearCacheWhenChangesAsync(entities.Select(x => (object)x.Id).ToList(), cancellationToken);
    }
    public async Task BulkDeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeDelete(entities);
        
        await _dbContext.BulkDeleteEntitiesAsync(entities, cancellationToken);

        await ClearCacheWhenChangesAsync(entities.Select(x => (object)x.Id).ToList(), cancellationToken);
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
    protected virtual async Task ClearCacheWhenChangesAsync(List<object> ids, CancellationToken cancellationToken = default)
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