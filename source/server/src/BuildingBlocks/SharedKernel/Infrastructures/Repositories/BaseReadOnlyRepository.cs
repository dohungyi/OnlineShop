using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Application.Consts;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.Persistence;
using SharedKernel.Properties;
using SharedKernel.Runtime.Exceptions;

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
        _dbContext = dbContext;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _provider = provider;
        
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

    public virtual async Task<IPagedList<TEntity>> GetPagingAsync(PagingRequest request, CancellationToken cancellationToken = default)
    {
        var localizer = _provider.GetRequiredService<IStringLocalizer<Resources>>();
        
        var query = _dbSet.Where(x => !x.IsDeleted).AsNoTracking();
        
        // Filter
        if (request.Filter is not null && request.Filter.Fields is not null && request.Filter.Fields.Any())
        {
            foreach (var field in request.Filter.Fields)
            {
                query = ApplyFilterConditions(query, field, localizer);
            }
        }
        
        // OrderBy
        if (request.Sorts is not null && request.Sorts.Any())
        {
            foreach (var sort in request.Sorts)
            {
                var property = typeof(TEntity).GetProperty(sort.FieldName);
                if (property is  null)
                {
                    throw new BadRequestException(localizer["repository_sorts_is_invalid"].Value);
                }
                
                if (sort.SortAscending)
                {
                    query = query.OrderBy(yourEntity => EF.Property<object>(yourEntity, sort.FieldName));
                }
                else
                {
                    query = query.OrderByDescending(yourEntity => EF.Property<object>(yourEntity, sort.FieldName));
                }
            }
        }
        
        return (await query.ToPagedListAsync(request.Page, request.Size, cancellationToken: cancellationToken));
    }

    #region [Private]
     private IQueryable<TEntity> ApplyFilterConditions(IQueryable<TEntity> query, Field field, IStringLocalizer<Resources> localizer)
    {
        var entityType = typeof(TEntity);
        var propertyName = field.FieldName;
        var property = typeof(TEntity).GetProperty(propertyName);

        if (property is null || string.IsNullOrWhiteSpace(field.Value))
        {
            throw new BadRequestException(localizer["repository_filter_is_invalid"].Value);
        }
        
        switch (field.Condition)
        {
            case WhereType.E:
                query = query.Where(entity => ((IComparable)property.GetValue(entity, null)).CompareTo(Convert.ChangeType(field.Value, property.PropertyType)) == 0);
                break;
            case WhereType.NE:
                query = query.Where(entity => ((IComparable)property.GetValue(entity, null)).CompareTo(Convert.ChangeType(field.Value, property.PropertyType)) != 0);
                break;
            case WhereType.GT:
                query = query.Where(entity => ((IComparable)property.GetValue(entity, null)).CompareTo(Convert.ChangeType(field.Value, property.PropertyType)) > 0);
                break;
            case WhereType.GE:
                query = query.Where(entity => ((IComparable)property.GetValue(entity, null)).CompareTo(Convert.ChangeType(field.Value, property.PropertyType)) >= 0);
                break;
            case WhereType.LT:
                query = query.Where(entity => ((IComparable)property.GetValue(entity, null)).CompareTo(Convert.ChangeType(field.Value, property.PropertyType)) < 0);
                break;
            case WhereType.LE:
                query = query.Where(entity => ((IComparable)property.GetValue(entity, null)).CompareTo(Convert.ChangeType(field.Value, property.PropertyType)) <= 0);
                break;
            case WhereType.C:
                query = query.Where(entity => EF.Functions.Like((string)entityType.GetProperty(propertyName).GetValue(entity, null), $"%{field.Value}%"));
                break;
            case WhereType.NC:
                query = query.Where(entity => !EF.Functions.Like((string)entityType.GetProperty(propertyName).GetValue(entity, null), $"%{field.Value}%"));
                break;
            case WhereType.SW:
                query = query.Where(entity => EF.Functions.Like((string)entityType.GetProperty(propertyName).GetValue(entity, null), $"{field.Value}%"));
                break;
            case WhereType.NSW:
                query = query.Where(entity => !EF.Functions.Like((string)entityType.GetProperty(propertyName).GetValue(entity, null), $"{field.Value}%"));
                break;
            case WhereType.EW:
                query = query.Where(entity => EF.Functions.Like((string)entityType.GetProperty(propertyName).GetValue(entity, null), $"%{field.Value}"));
                break;
            case WhereType.NEW:
                query = query.Where(entity => !EF.Functions.Like((string)entityType.GetProperty(propertyName).GetValue(entity, null), $"%{field.Value}"));
                break;
        }

        return query;
    }
    
    #endregion
}