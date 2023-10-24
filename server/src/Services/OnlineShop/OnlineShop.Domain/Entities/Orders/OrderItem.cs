namespace OnlineShop.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
        
    public Guid ProductId { get; set; }
    public Guid PromotionId { get; set; }
    public Guid OrderId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public virtual Product Product { get; set; }
    public virtual Order Order { get; set; }
    #endregion
}