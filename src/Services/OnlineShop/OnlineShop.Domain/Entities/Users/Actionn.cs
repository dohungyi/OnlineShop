using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Actionn : EntityBase<Guid>
{
    public string Title { get; set; }
    public string Module { get; set; }
    public string Action { get; set; }
    public string ProfileTypes { get; set; }
}