using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Application;

public interface IBaseWriteOnlyRepository<TEntity, TDbContext> 
    where TEntity : BaseEntity
    where TDbContext : DbContext
{
    Task PublishEvents(IEventBus eventBus, CancellationToken cancellationToken);
    
    TEntity Insert(TEntity entity);
        
    IList<TEntity> Insert(IList<TEntity> entities);

    IList<TEntity> BulkInsert(IList<TEntity> listEntities);
        
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        
    Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task<IList<TEntity>> BulkInsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
        
    void Update(TEntity entity);

    void Delete(TEntity entity);

    void Delete(IList<TEntity> entities);

    void BulkDelete(IList<TEntity> listEntities);

    Task BulkDeleteAsync(IList<TEntity> listEntities, CancellationToken cancellationToken = default);
}