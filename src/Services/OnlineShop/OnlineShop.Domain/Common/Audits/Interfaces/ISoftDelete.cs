namespace OnlineShop.Domain.Common.Audits.Interfaces;

public interface ISoftDelete
{
    public bool? IsDeleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}