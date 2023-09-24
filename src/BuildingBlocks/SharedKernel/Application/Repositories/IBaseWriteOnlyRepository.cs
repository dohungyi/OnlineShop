using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Application;

public interface IBaseWriteOnlyRepository<TEntity, TDbContext> 
    where TEntity : BaseEntity
    where TDbContext : DbContext
{
    Task PublishEvents(IEventBus eventBus, CancellationToken cancellationToken);
    
    void Insert(TEntity entity);
        
    void Insert(IList<TEntity> entities);

    void BulkInsert(IList<TEntity> listEntities);
        
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        
    Task InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task BulkInsertAsync(IList<TEntity> listEntities, CancellationToken cancellationToken = default);
        
    void Update(TEntity entity);
        
    void Update(IList<TEntity> entities);

    void BulkUpdate(IList<TEntity> listEntities);

    Task BulkUpdateAsync(IList<TEntity> listEntities, CancellationToken cancellationToken = default);

    void Delete(TEntity entity);

    void Delete(IList<TEntity> entities);

    void BulkDelete(IList<TEntity> listEntities);

    Task BulkDeleteAsync(IList<TEntity> listEntities, CancellationToken cancellationToken = default);
}