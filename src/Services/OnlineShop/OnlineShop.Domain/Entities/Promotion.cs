namespace OnlineShop.Domain.Entities;

public class Promotion : BaseEntity
{
    public string Code { get; set; }
    public double DiscountPercent { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public ICollection<Order> Orders;
    #endregion
}