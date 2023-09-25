using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Ratting : EntityAuditBase<Guid>
{
    public double StarPoint { get; set; }
    
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public virtual User User { get; set; }
    public virtual Product Product { get; set; }
    #endregion
}