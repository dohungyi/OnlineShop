namespace OnlineShop.Domain.Entities;

public class Avatar : BaseEntity
{
    public string FileName { get; set; }
    
    public Guid UserId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual ApplicationUser User { get; set; }
    
    #endregion
}