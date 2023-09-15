using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;
using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.Libraries.Utility;

namespace SharedKernel.Infrastructures.Repositories;

public class BaseWriteOnlyRepository<TEntity, TDbContext> : IBaseWriteOnlyRepository<TEntity, TDbContext>
    where TEntity :  BaseEntity
    where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly ICurrentUser _currentUser;
    private List<DomainEvent> DomainEvents { get; set; } = new();

    public BaseWriteOnlyRepository(TDbContext dbContext, ICurrentUser currentUser)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = dbContext.Set<TEntity>();
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    
    public void Insert(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Insert(IList<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public void BulkInsert<TEntity>(IList<TEntity> listEntities)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task BulkInsertAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Update(IList<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public void BulkUpdate<TEntity>(IList<TEntity> listEntities) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public Task BulkUpdateAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Delete(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(IList<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public void BulkDelete<TEntity>(IList<TEntity> listEntities) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public Task BulkDeleteAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public bool SaveChanges()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}