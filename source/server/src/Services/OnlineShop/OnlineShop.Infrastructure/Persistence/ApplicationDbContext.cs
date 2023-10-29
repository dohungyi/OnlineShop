using System.Reflection;
using Action = OnlineShop.Domain.Entities.Action;

namespace OnlineShop.Infrastructure.Persistence;

public class ApplicationDbContext : AppDbContext, IApplicationDbContext
{
    #region [CONSTRUCTOR]
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public ApplicationDbContext(string connectionString) : base(new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString).Options)
    {
        
    }
    
    #endregion
    
    #region [DB SET]

    #region [USERS]

    public DbSet<RequestInformation> RequestInformations { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Avatar> Avatars { get; set; }
    public DbSet<ApplicationUserConfig> ApplicationUserConfigs { get; set; }
    public DbSet<ApplicationUserAddress> ApplicationUserAddresses { get; set; }
    public DbSet<ApplicationUserPayment> ApplicationUserPayments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<RoleAction> RoleActions { get; set; }
    public DbSet<SignInHistory> SignInHistories { get; set; }

    #endregion [USERS]
    
    #endregion
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}