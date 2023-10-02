namespace OnlineShop.Domain.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string? ImageFileName { get; set; }
    public string Description { get; set; }

    #region [REFRENCE PROPERTIES]
    public ICollection<Product> Products;
    #endregion
}