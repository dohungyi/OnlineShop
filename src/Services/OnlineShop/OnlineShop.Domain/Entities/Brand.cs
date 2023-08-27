using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Brand : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string? OriginLinkImage { get; set; }
    public string LocalLinkImage { get; set; }
    public string Description { get; set; }

    #region [REFRENCE PROPERTIES]
    public ICollection<Product> Products;
    #endregion
}