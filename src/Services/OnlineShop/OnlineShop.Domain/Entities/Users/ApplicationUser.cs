namespace OnlineShop.Domain.Entities;

public class ApplicationUser : User
{
    #region [REFRENCE PROPERTIES]
    
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Order> Orders { get; set; }

    #endregion
}