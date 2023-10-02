namespace OnlineShop.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public string Type { get; set; }
    
    #region [REFRENCE PROPERTIES]

    public ICollection<ProductTag> ProductTags { get; set; }

    #endregion
}
