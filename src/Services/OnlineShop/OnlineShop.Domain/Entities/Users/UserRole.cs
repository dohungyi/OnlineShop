namespace OnlineShop.Domain.Entities;

public class UserRole : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    #region [REFRENCE PROPERTIES]
    public User User { get; set; }
    public Role Role { get; set; }
    #endregion
}



