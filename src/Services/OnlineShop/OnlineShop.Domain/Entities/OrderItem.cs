using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class OrderItem : EntityAuditBase<Guid>
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
        
    public Guid ProductId { get; set; }
    public Guid PromotionId { get; set; }
    public Guid OrderId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public virtual Product Product { get; set; }
    public virtual Promotion Promotion { get; set; }
    public virtual Order Order { get; set; }
    #endregion
}