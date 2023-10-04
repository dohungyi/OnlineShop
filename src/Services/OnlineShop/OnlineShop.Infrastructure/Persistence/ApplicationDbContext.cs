using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Entities;
using SharedKernel.Persistence;
using Action = OnlineShop.Domain.Entities.Action;

namespace OnlineShop.Infrastructure.Persistence;

public class ApplicationDbContext : AppDbContext
{
    #region [CONSTRUCTOR]
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base()
    {
        
    }
    #endregion
    
    #region [DB SET]

    #region [USERS]

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Avatar> Avatars { get; set; }
    public DbSet<ApplicationUserConfig> ApplicationUserConfigs { get; set; }
    public DbSet<ApplicationUserAddress> ApplicationUserAddresses { get; set; }
    public DbSet<ApplicationUserPayment> ApplicationUserPayments { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<RoleAction> RoleActions { get; set; }

    #endregion [USERS]
    
    #endregion
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}