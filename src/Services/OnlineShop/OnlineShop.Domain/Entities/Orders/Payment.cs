namespace OnlineShop.Domain.Entities;

public class Payment : BaseEntity
{
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionId { get; set; }
    public string PaymemtCode { get; set; }
    public string TransactionDate { get; set; }
    public string Free { get; set; }
    
    public Guid OrderStatusId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public virtual OrderStatus OrderStatus { get; set; }
    public virtual Order Order { get; set; }
    #endregion
}