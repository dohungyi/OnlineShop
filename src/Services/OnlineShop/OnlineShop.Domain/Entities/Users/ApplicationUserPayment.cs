namespace OnlineShop.Domain.Entities;

public class ApplicationUserPayment : BaseEntity
{
    // ...
    public Guid UserId { get; set; }
    
    #region [REFRENCE PROPERTIES]

    public virtual ApplicationUser User { get; set; }

    #endregion
}