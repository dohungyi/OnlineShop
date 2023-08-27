namespace OnlineShop.Domain.Entities;

public class Slide
{
    public string Name { set; get; }
    public string Description { set; get; }
    public string Image { set; get; }
    public string Url { set; get; }
    public int? DisplayOrder { set; get; }
    public bool Status { set; get; }
    public string Content { set; get; }
    
    public Guid SlideGroupId { set; get; }

    #region [REFRENCE PROPERTIES]
    public virtual SlideGroup SlideGroup { set; get; }
    #endregion

}