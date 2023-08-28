namespace OnlineShop.Domain.Entities;

public class Slide
{
    public string Name { set; get; }
    public string Description { set; get; }
    public string Content { set; get; }
    public string? OriginLinkImage { get; set; }
    public string LocalLinkImage { get; set; }
    public int? DisplayOrder { set; get; }
    public bool Status { set; get; }
    
    public Guid SlideGroupId { set; get; }

    #region [REFRENCE PROPERTIES]
    public virtual SlideGroup SlideGroup { set; get; }
    #endregion

}