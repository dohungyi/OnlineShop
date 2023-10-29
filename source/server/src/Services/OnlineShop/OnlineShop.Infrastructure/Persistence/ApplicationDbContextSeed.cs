using Microsoft.AspNetCore.Http;
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
        var sa = GetSupperAdmin();
        var admin = GetAdmin();
        var roles = GetRoles();
        var actions = GetActions();
        var supperAdminRole = roles.FirstOrDefault(r => r.Code == RoleConstant.SupperAdmin);
        var adminRole = roles.FirstOrDefault(r => r.Code == RoleConstant.Admin);
        
        if (!_context.ApplicationUsers.Any())
        {
            // add role
            foreach (var role in roles)
            {
                await _context.Roles.AddAsync(role);
            }
            
            // add action
            foreach (var action in actions)
            {
                await _context.Actions.AddAsync(action);
            }
            
            // add role action
            foreach (var action in actions)
            {
                var roleAction = new RoleAction
                {
                    Id = Guid.NewGuid(),
                    Role = supperAdminRole,
                    Action = action,
                    IsDeleted = false, 
                    CreatedDate = DateHelper.Now, 
                    CreatedBy = "supperadmin", 
                    LastModifiedDate = null, 
                    LastModifiedBy = null, 
                    DeletedDate = null, 
                    DeletedBy = null
                };
            
                await _context.RoleActions.AddAsync(roleAction);

                if (action.Exponent <= (int)ActionExponent.Admin)
                {
                    var roleAction_ = new RoleAction
                    {
                        Id = Guid.NewGuid(),
                        Role = adminRole,
                        Action = action,
                        IsDeleted = false, 
                        CreatedDate = DateHelper.Now, 
                        CreatedBy = "supperadmin", 
                        LastModifiedDate = null, 
                        LastModifiedBy = null, 
                        DeletedDate = null, 
                        DeletedBy = null
                    };
            
                    await _context.RoleActions.AddAsync(roleAction_);
                }
            }
            
            // add user
            await _context.ApplicationUsers.AddAsync(sa);
            await _context.ApplicationUsers.AddAsync(admin);
            
            // add user role
            var uRSA = new UserRole()
            {
                Id = Guid.NewGuid(),
                Role = supperAdminRole,
                User = sa,
                IsDeleted = false,
                CreatedDate = DateHelper.Now,
                CreatedBy = "supperadmin",
                LastModifiedDate = null,
                LastModifiedBy = null,
                DeletedDate = null,
                DeletedBy = null
            };
            
            var uRA = new UserRole()
            {
                Id = Guid.NewGuid(),
                Role = adminRole,
                User = admin,
                IsDeleted = false,
                CreatedDate = DateHelper.Now,
                CreatedBy = "supperadmin",
                LastModifiedDate = null,
                LastModifiedBy = null,
                DeletedDate = null,
                DeletedBy = null
            };
            await _context.UserRoles.AddAsync(uRSA);
            await _context.UserRoles.AddAsync(uRA);
        }
        
        await _context.CommitAsync(false);
    }

    private ApplicationUser GetAdmin()
    {
        return new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            PasswordHash = "admin".ToMD5(),
            Salt = Utility.RandomString(6),
            PhoneNumber = "09897001xx",
            ConfirmedPhone = true,
            Email = "admin.csharp@gmail.com",
            ConfirmedEmail = true,
            FirstName = "Đỗ Chí",
            LastName = "Hòa",
            Address = "Đông Kết, Khoái Châu, Hưng Yên",
            DateOfBirth = new DateTime(1990, 01, 01).ToUniversalTime(),
            Gender = GenderType.Other,
            CreatedDate = DateHelper.Now,
            CreatedBy = "supperadmin",
            LastModifiedDate = null,
            LastModifiedBy = null,
            DeletedDate = null,
            DeletedBy = null,
            IsDeleted = false
        };
    }
    
    private ApplicationUser GetSupperAdmin()
    {
        return new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            Username = "supperadmin",
            PasswordHash = "supperadmin".ToMD5(),
            Salt = Utility.RandomString(6),
            PhoneNumber = "0976580418",
            ConfirmedPhone = true,
            Email = "dohung.csharp@gmail.com",
            ConfirmedEmail = true,
            FirstName = "Đỗ Chí",
            LastName = "Hùng",
            Address = "Đông Kết, Khoái Châu, Hưng Yên",
            DateOfBirth = new DateTime(2002, 9, 6).ToUniversalTime(),
            Gender = GenderType.Other,
            CreatedDate = DateHelper.Now,
            CreatedBy = "supperadmin",
            LastModifiedDate = null,
            LastModifiedBy = null,
            DeletedDate = null,
            DeletedBy = null,
            IsDeleted = false
        };
    }

    private IEnumerable<Role> GetRoles()
    {
        return new List<Role>()
        {
            new Role() { Code = RoleConstant.SupperAdmin, Name = "Super Admin", IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Role() { Code = RoleConstant.Admin, Name = "Admin", IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Role() { Code = RoleConstant.Customer, Name = "Customer", IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null }
        };
    }

    private IEnumerable<Action> GetActions()
    {
        var actions = new List<Action>()
        {
            new Action() { Code = "ALLOW_ANONYMOUS", Name = "Allow Anonymous", Exponent = (int)ActionExponent.AllowAnonymous, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "SUPPER_ADMIN", Name = "Supper Admin", Exponent = (int)ActionExponent.SupperAdmin, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "ADMIN", Name = "Admin", Exponent = (int)ActionExponent.Admin, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "VIEW", Name = "View", Exponent = (int)ActionExponent.View, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "ADD", Name = "Add", Exponent = (int)ActionExponent.Add, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "EDIT", Name = "Edit", Exponent = (int)ActionExponent.Edit, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "DELETE", Name = "Delete", Exponent = (int)ActionExponent.Delete, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "EXPORT", Name = "Export", Exponent = (int)ActionExponent.Export, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "IMPORT", Name = "Import", Exponent = (int)ActionExponent.Import, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "UPLOAD", Name = "Upload", Exponent = (int)ActionExponent.Upload, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
            new Action() { Code = "DOWNLOAD", Name = "Download", Exponent = (int)ActionExponent.Download, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = "supperadmin", LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null },
        };

        return actions;
    }
    
    #endregion
}