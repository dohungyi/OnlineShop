namespace OnlineShop.Domain.Entities;

public class Property : BaseEntity
{
    public string Name { get; set; }
    public string Value { get; set; }
    
    public Guid ProductId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    public Product Product { set; get; }
    #endregion
}