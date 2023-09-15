using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Application.Consts;
using SharedKernel.Application.Responses;
using SharedKernel.Domain;

namespace SharedKernel.Application;

public interface IBaseReadOnlyRepository<TEntity, TDbContext> 
    where TEntity :  BaseEntity
    where TDbContext : DbContext
{
    IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true);
    
    IPagedList<TEntity> PagingAll(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        int pageIndex = Default.PageIndex,
        int pageSize = Default.PageSize,
        int indexFrom = Default.IndexFrom);

    Task<IPagedList<TEntity>> PagingAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        int pageIndex = Default.PageIndex,
        int pageSize = Default.PageSize,
        int indexFrom = Default.IndexFrom,
        CancellationToken cancellationToken = default);
    
   TEntity Get(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true);

    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);
}