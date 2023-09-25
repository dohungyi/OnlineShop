using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class RoleAction : EntityBase<Guid>
{
    
    public Guid GroupId { get; set; }
    public Guid PermId { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public Role Role { get; set; }
    public Actionn Actionn { get; set; }
    #endregion
}