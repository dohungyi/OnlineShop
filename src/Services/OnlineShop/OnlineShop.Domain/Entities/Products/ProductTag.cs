namespace OnlineShop.Domain.Entities;

public class ProductTag : BaseEntity
{
    public int ProductId { set; get; }
    
    public string TagId { set; get; }

    #region [REFRENCE PROPERTIES]
    public virtual Product Product { set; get; }
    public virtual Tag Tag { set; get; }
    #endregion
}