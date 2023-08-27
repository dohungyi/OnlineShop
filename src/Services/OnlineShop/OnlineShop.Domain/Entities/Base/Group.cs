using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities.Base;

public class Group : EntityBase<Guid>
{
    public string Title { get; set; }
    public string ProfileType { get; set; }
    public string Description { get; set; }
}