using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Entities;

public class Rating : BaseEntity
{
    public double StarPoint { get; set; }
    
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public virtual ApplicationUser User { get; set; }
    public virtual Product Product { get; set; }
    #endregion
}