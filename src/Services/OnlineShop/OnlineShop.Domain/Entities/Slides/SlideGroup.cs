namespace OnlineShop.Domain.Entities;

public class SlideGroup : BaseEntity
{
    public string Name { set; get; }

    #region [REFRENCE PROPERTIES]
    public ICollection<Slide> Slides { set; get; }
    #endregion

}