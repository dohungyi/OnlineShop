using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities.Base;

public class Permission : EntityBase<Guid>
{
    public string Title { get; set; }
    public string Module { get; set; }
    public string Action { get; set; }
    public string ProfileTypes { get; set; }
}