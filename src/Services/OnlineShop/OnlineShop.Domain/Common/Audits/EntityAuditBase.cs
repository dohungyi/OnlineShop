using OnlineShop.Domain.Common.Audits.Interfaces;

namespace OnlineShop.Domain.Common.Audits;

public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable
{
    public DateTime CreateDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}