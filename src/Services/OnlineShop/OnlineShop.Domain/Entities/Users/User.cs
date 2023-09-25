using OnlineShop.Domain.Common.Audits;

namespace OnlineShop.Domain.Entities;

public class User : EntityBase<Guid>
{
    public string UserName { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string GroupIds { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? HashRefreshToken { get; set; }
}