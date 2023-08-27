using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Promotion : EntityAuditBase<Guid>
{
    public string Code { get; set; }
    public double DiscountPercent { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public ICollection<OrderItem> OrderItems;
    #endregion
}