using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using SharedKernel.Auth;
using SharedKernel.Log;
using Action = OnlineShop.Domain.Entities.Action;

namespace OnlineShop.Infrastructure.Persistence;

public class ApplicationDbContextSeed
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _accessor;
    public ApplicationDbContextSeed(ApplicationDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }
    
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
                await _context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            Logging.Error("An error occurred while initialising the database.");
            throw;
        }
    }
    
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await _context.CommitAsync(false);
        }
        catch (Exception e)
        {
            Logging.Error("An error occurred while seeding the database;");
            throw;
        }
    }


    #region [Private]
    
    private async Task TrySeedAsync()
    {
        var roles = GetRoles();
        var actions = GetActions();

        if (!_context.Roles.Any())
        {
            foreach (var role in roles)
            {
                role.AddDomainEvent(new InsertAuditEvent<Role>(new List<Role> { role }, new CurrentUser(_accessor){}));
                await _context.Roles.AddAsync(role);
            }
        }

        if (!_context.Actions.Any())
        {
            foreach (var action in actions)
            {
                await _context.Actions.AddAsync(action);
            }
        }

        var supperAdmin = roles.FirstOrDefault(r => r.Code == "SUPPER_ADMIN");

        if (supperAdmin is not null)
        {
            foreach (var action in actions)
            {
                var roleAction = new RoleAction
                {
                    Id = Guid.NewGuid(),
                    Role = supperAdmin,
                    Action = action,
                    IsDeleted = false, 
                    CreatedDate = SharedKernel.Libraries.DateHelper.Now, 
                    CreatedBy = "admin", 
                    LastModifiedDate = null, 
                    LastModifiedBy = null, 
                    DeletedDate = null, 
                    DeletedBy = null
                };
                
                await _context.RoleActions.AddAsync(roleAction);
                
            }
        }
        
        await _context.SaveChangesAsync();
    }

    private IEnumerable<Role> GetRoles()
    {
        return new List<Role>()
        {
            new Role() { Code = "SUPPER_ADMIN", Name = "Super Admin", IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Role() { Code = "ADMIN", Name = "Admin", IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Role() { Code = "STAFF", Name = "Staff", IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
        };
    }

    private IEnumerable<Action> GetActions()
    {
        var actions = new List<Action>()
        {
            new Action() { Code = "AllowAnonymous", Name = "Allow Anonymous", Exponent = (int)ActionExponent.AllowAnonymous, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Staff", Name = "Staff", Exponent = (int)ActionExponent.Staff, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Admin", Name = "Admin", Exponent = (int)ActionExponent.Admin, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "View", Name = "View", Exponent = (int)ActionExponent.View, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Add", Name = "Add", Exponent = (int)ActionExponent.Add, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Edit", Name = "Edit", Exponent = (int)ActionExponent.Edit, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Delete", Name = "Delete", Exponent = (int)ActionExponent.Delete, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Export", Name = "Export", Exponent = (int)ActionExponent.Export, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Import", Name = "Import", Exponent = (int)ActionExponent.Import, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Upload", Name = "Upload", Exponent = (int)ActionExponent.Upload, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "Download", Name = "Download", Exponent = (int)ActionExponent.Download, IsDeleted = false, CreatedDate = SharedKernel.Libraries.DateHelper.Now, CreatedBy = "admin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
        };

        return actions;
    }
    
    #endregion
}