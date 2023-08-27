using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class OrderStatus : EntityAuditBase<Guid>
{
    public string Type { get; set; }
    public string Display { get; set; }
    public string Code { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public ICollection<Order> Orders { get; set; }
    public ICollection<Payment> Payments { get; set; }
    #endregion
}