using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Application.Consts;
using SharedKernel.Application.Responses;
using SharedKernel.Domain;

namespace SharedKernel.Application;

public interface IBaseReadOnlyRepository<TEntity, TKey, TDbContext> 
    where TEntity :  BaseEntity<TKey>
    where TDbContext : DbContext
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
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties);
}