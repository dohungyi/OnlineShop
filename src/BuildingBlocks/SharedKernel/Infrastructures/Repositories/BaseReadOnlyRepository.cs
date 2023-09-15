using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Application;
using SharedKernel.Application.Responses;
using SharedKernel.Domain;

namespace SharedKernel.Infrastructures.Repositories;

public class BaseReadOnlyRepository<TEntity, TKey, TDbContext> : IBaseReadOnlyRepository<TEntity, TDbContext> 
    where TEntity :  BaseEntity
    where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseReadOnlyRepository(TDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = dbContext.Set<TEntity>();
    }
    
    public IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
        => Query(predicate, orderBy, include, disableTracking);

    public IPagedList<TEntity> PagingAll(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        int pageIndex = 1,
        int pageSize = 10, 
        int indexFrom = 1)
        => Query(predicate, orderBy, include, disableTracking).ToPagedList(pageIndex, pageSize, indexFrom);
    
    public async Task<IPagedList<TEntity>> PagingAllAsync(Expression<Func<TEntity, bool>> predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, 
        bool disableTracking = true,
        int pageIndex = 1, 
        int pageSize = 10, 
        int indexFrom = 1, 
        CancellationToken cancellationToken = default)
        => await Query(predicate, orderBy, include, disableTracking).ToPagedListAsync(pageIndex, pageSize, indexFrom, cancellationToken);

    public TEntity Get(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
        => Query(predicate, orderBy, include, disableTracking).FirstOrDefault();
    
    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default)
        => await Query(predicate, orderBy, include, disableTracking).FirstOrDefaultAsync(cancellationToken);
    
    #region [Private Methods]
    private IQueryable<TEntity> Query(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (include != null)
            query = include(query);

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        return query;
    }
    #endregion
}