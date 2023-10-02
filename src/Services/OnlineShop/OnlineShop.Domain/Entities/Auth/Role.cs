namespace OnlineShop.Domain.Entities;

public class Role : BaseEntity
{
    public string Code { get; set; }

    public string Name { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RoleAction> RoleActions { get; set; }
    
    #endregion
}