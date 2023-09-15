using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Application;

public interface IBaseWriteOnlyRepository<TEntity, TDbContext> 
    where TEntity : BaseEntity
    where TDbContext : DbContext
{
    void Insert(TEntity entity);
        
    void Insert(IList<TEntity> entities);

    void BulkInsert<TEntity>(IList<TEntity> listEntities);
        
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        
    Task InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task BulkInsertAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default);
        
    void Update(TEntity entity);
        
    void Update(IList<TEntity> entities);
    
    void BulkUpdate<TEntity>(IList<TEntity> listEntities) where TEntity : class;

    Task BulkUpdateAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default);

    void Delete(TEntity entity);

    void Delete(IList<TEntity> entities);
    
    void BulkDelete<TEntity>(IList<TEntity> listEntities) where TEntity : class;

    Task BulkDeleteAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default);
    
    bool SaveChanges();
    
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}