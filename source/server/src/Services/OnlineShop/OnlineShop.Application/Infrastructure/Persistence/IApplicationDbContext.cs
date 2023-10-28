using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Persistence;
using Action = OnlineShop.Domain.Entities.Action;

namespace OnlineShop.Application.Infrastructure;

public interface IApplicationDbContext : IAppDbContext
{
    DbSet<RequestInformation> RequestInformations { get; set; }
    DbSet<ApplicationUser> ApplicationUsers { get; set; }
    DbSet<Avatar> Avatars { get; set; }
    DbSet<ApplicationUserConfig> ApplicationUserConfigs { get; set; }
    DbSet<ApplicationUserAddress> ApplicationUserAddresses { get; set; }
    DbSet<ApplicationUserPayment> ApplicationUserPayments { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<Action> Actions { get; set; }
    DbSet<RoleAction> RoleActions { get; set; }
    DbSet<SignInHistory> SignInHistories { get; set; }
}