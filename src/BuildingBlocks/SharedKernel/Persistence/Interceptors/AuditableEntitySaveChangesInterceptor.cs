using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Libraries;

namespace SharedKernel.ApplicationDbContext;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;

    public AuditableEntitySaveChangesInterceptor(ICurrentUser currentUser)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        };

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUser.Context.Username;
                entry.Entity.CreatedDate = DateHelper.Now;
            } 

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = _currentUser.Context.Username;
                entry.Entity.LastModifiedDate = DateHelper.Now;
            }
            
            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.DeletedBy = _currentUser.Context.Username;
                entry.Entity.DeletedDate = DateHelper.Now;
                entry.State = EntityState.Modified;
            }
        }
    }
    
}