using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class UserGroup : EntityBase<Guid>
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }

    #region [REFRENCE PROPERTIES]
    public User User { get; set; }
    public Role Role { get; set; }
    #endregion
}