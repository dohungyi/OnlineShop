using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;
using SharedKernel.Application.Consts;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Domain;
using SharedKernel.Persistence;

namespace SharedKernel.Infrastructures.Repositories;

public class BaseReadOnlyRepository<TEntity, TDbContext> : IBaseReadOnlyRepository<TEntity, TDbContext>
    where TEntity :  BaseEntity
    where TDbContext : AppDbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly IServiceProvider _provider;
    protected readonly string _tableName;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseReadOnlyRepository(
        TDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _sequenceCaching = sequenceCaching ?? throw new ArgumentNullException(nameof(sequenceCaching));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        
        _tableName = nameof(TEntity);
        _dbSet = dbContext.Set<TEntity>();

    }

    #region [CACHE]
    
    public virtual async Task<List<TEntity>> GetAllCacheAsync(CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
        
        return await _sequenceCaching.GetAsync<List<TEntity>>(key, cancellationToken: cancellationToken);
    }
    

    public virtual async Task<TEntity> GetByIdCacheAsync(object id, CancellationToken cancellationToken)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id);
        
        return await _sequenceCaching.GetAsync<TEntity>(key, cancellationToken: cancellationToken);
    }
    
    #endregion

    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        return !trackChanges ? _dbSet.AsNoTracking() : _dbSet;
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var queryable = FindAll(trackChanges);
        queryable = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        return queryable;
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges ? _dbSet.Where(expression).AsNoTracking() : _dbSet.Where(expression);
    }
    
    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var queryable = FindByCondition(expression, trackChanges);
        queryable = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        return queryable;
    }
    
    public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        var cacheResult = await GetByIdCacheAsync(id, cancellationToken);
        if (cacheResult is not null)
        {
            return cacheResult;
        }
        
        return await FindByCondition(x => x.Id.Equals(id))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(object id,  CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
         return await FindByCondition(x => x.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync(cancellationToken);
    }
}