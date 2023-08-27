using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class Tag : EntityBase<Guid>
{
    public string Name { get; set; }
    public string Type { get; set; }
}
