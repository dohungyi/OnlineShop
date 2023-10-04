using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Entities;

public class ApplicationUser : User
{
    
    public Guid AvatarId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual Avatar Avatar { get; set; }
    public virtual ApplicationUserConfig UserConfig { get; set; }
    public ICollection<ApplicationUserAddress> UserAddresses { get; set; }
    public ICollection<ApplicationUserPayment> UserPayments { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Order> Orders { get; set; }

    #endregion
}