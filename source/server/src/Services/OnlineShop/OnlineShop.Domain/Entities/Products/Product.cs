namespace OnlineShop.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    
    public decimal OriginalPrice { set; get; }
    public decimal Price { get; set; } 
    
    public double Discount { get; set; }
    public string Currency { get; set; }
    
    public Guid DefaultImage { get; set; }
    public string ImageFileName { get; set; }
    
    public string Url { get; set; }
    public string Stock { get; set; }
    
    public bool? HomeFlag { get; set; }
    public bool? HotFlag { get; set; }
    public bool? IsBestSelling { get; set; }
    public bool? IsNew { get; set; }
    public int? ViewCount { get; set; }
    
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }

    #region [REFRENCE PROPERTIES]
    
    public virtual Brand Brand { get; set; }
    public virtual Category Category { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Rating> Rattings { get; set; }
    public ICollection<Property> Properties { get; set; }
    #endregion
}