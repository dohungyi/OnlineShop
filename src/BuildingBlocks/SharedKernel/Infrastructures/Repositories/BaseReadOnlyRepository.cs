using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Application;
using SharedKernel.Application.Responses;
using SharedKernel.Domain;

namespace SharedKernel.Infrastructures.Repositories;

public class BaseReadOnlyRepository<TEntity, TKey, TDbContext> : IBaseReadOnlyRepository<TEntity, TKey, TDbContext>
    where TEntity :  BaseEntity<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseReadOnlyRepository(TDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        return !trackChanges ? _dbSet.AsNoTracking() : _dbSet;
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges ? _dbSet.Where(expression).AsNoTracking() : _dbSet.Where(expression);
    }
        

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await FindByCondition(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
         return await FindByCondition(x => x.Id.Equals(id), trackChanges:false, includeProperties)
            .FirstOrDefaultAsync();
    }
}