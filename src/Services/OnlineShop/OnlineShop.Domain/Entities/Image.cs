namespace OnlineShop.Domain.Entities;

public class Image : BaseEntity
{
    public string? OriginLinkImage { get; set; }
    public string LocalLinkImage { get; set; }
    
    public Guid ProductId { get; set; }

    #region [REFRENCE PROPERTIES]
    public virtual Product Product { get; set; }
    #endregion
}