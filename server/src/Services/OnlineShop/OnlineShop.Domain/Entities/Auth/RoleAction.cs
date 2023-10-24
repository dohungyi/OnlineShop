namespace OnlineShop.Domain.Entities;

public class RoleAction : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid ActionId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public Role Role { get; set; }
    public Action Action { get; set; }
    
    #endregion
}