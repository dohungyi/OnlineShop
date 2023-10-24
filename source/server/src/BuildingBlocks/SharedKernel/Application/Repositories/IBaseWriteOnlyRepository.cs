using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Persistence;

namespace SharedKernel.Application;

public interface IBaseWriteOnlyRepository<TEntity,TDbContext> 
    where TEntity : BaseEntity
    where TDbContext : IAppDbContext
{
    IUnitOfWork UnitOfWork { get; }
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    Task<IList<TEntity>> BulkInsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken);
    Task BulkDeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
}