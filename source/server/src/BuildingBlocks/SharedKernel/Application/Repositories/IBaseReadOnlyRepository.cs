using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Persistence;

namespace SharedKernel.Application;

public interface IBaseReadOnlyRepository<TEntity, TDbContext> 
    where TEntity : BaseEntity
    where TDbContext : IAppDbContext
{
    
    #region [CACHE]
    
    Task<List<TEntity>> GetAllCacheAsync(CancellationToken cancellationToken = default);

    Task<TEntity> GetByIdCacheAsync(object id, CancellationToken cancellationToken);

    #endregion
    
    IQueryable<TEntity> FindAll(bool trackChanges = false);
    IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includeProperties);
    Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IPagedList<TEntity>> GetPagingAsync(PagingRequest request, CancellationToken cancellationToken = default);
}