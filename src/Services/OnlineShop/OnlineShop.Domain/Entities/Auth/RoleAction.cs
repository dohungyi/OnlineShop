namespace OnlineShop.Domain.Entities;

[Table("common_role_action")]
public class RoleAction : BaseEntity
{
    public int RoleId { get; set; }
    public long ActionId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public Role Role { get; set; }
    public Action Action { get; set; }
    
    #endregion
}