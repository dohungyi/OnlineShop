using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Role : EntityBase<Guid>
{
    public string Title { get; set; }
    public string ProfileType { get; set; }
    public string Description { get; set; }
}