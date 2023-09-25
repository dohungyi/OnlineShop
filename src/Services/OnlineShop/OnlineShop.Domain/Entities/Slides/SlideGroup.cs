using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class SlideGroup : EntityBase<Guid>
{
    public string Name { set; get; }

    #region [REFRENCE PROPERTIES]
    public ICollection<Slide> Slides { set; get; }
    #endregion

}