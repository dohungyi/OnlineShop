namespace OnlineShop.Domain.Entities;

public class UserRole : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    #region [REFRENCE PROPERTIES]
    public virtual ApplicationUser User { get; set; }
    public virtual Role Role { get; set; }
    #endregion
}



