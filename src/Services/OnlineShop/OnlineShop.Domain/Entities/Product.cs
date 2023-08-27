using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Product : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    
    public decimal OriginalPrice { set; get; }// giá gốc
    public decimal Price { get; set; } // giá bán
    
    public double Discount { get; set; }
    public string Currency { get; set; }
    
    public Guid DefaultImage { get; set; }
    public string OriginLinkDetail { get; set; }
    
    public string Url { get; set; }
    public string Stock { get; set; }
    
    public bool? HomeFlag { get; set; }
    public bool? HotFlag { get; set; }
    public int? ViewCount { get; set; }
    public string Tags { set; get; }
    
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }

    #region [REFRENCE PROPERTIES]
    public virtual Brand Brand { get; set; }
    public virtual Category Category { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Ratting> Rattings { get; set; }
    #endregion
}