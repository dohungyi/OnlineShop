using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities.Bases;

public class GroupPermission : EntityBase<Guid>
{
    
    public Guid GroupId { get; set; }
    public Guid PermId { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public Group Group { get; set; }
    public Permission Permission { get; set; }
    #endregion
}