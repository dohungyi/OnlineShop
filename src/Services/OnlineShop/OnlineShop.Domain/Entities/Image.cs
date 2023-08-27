using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Image : EntityAuditBase<Guid>
{
    public string? OriginLinkImage { get; set; }
    public string LocalLinkImage { get; set; }
    
    public Guid ProductId { get; set; }

    #region [REFRENCE PROPERTIES]
    public virtual Product Product { get; set; }
    #endregion
}