using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Entities;

public class Order : BaseEntity
{
    public string Code { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string Note { get; set; }
    public bool CancelReturn { get; set; }
    
    public Guid PaymentId { get; set; } 
    public Guid StatusId { get; set; }
    public Guid UserId { get; set; }
    public Guid? PromotionId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public virtual ApplicationUser User { get; set; }
    public virtual Payment Payment { get; set; }
    public virtual Promotion Promotion { get; set; }
    public virtual OrderStatus OrderStatus { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    #endregion
}