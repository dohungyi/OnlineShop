using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Persistence;

namespace OnlineShop.Audit.Persistence;

public class IntegrationAuditDbContext : AppDbContext
{
    public IntegrationAuditDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<AuditEntity> AuditEntities { get; set; }


    public override Task BulkInsertEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return this.BulkInsertAsync(entities, cancellationToken: cancellationToken);
    }
    
    public override Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return this.BulkInsertAsync(entities, cancellationToken: cancellationToken);
    }

    public override Task BulkCommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        return this.BulkSaveChangesAsync(cancellationToken: cancellationToken);
    }

    public override Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }

    protected override void ProcessDomainEvents()
    {
        
    }
}