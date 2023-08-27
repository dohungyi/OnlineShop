namespace OnlineShop.Domain.Common.Audits.Interfaces;

public interface IDateTracking
{
    DateTime CreateDate { get; set; }
    DateTime? LastModifiedDate { get; set; }
}