namespace OnlineShop.Domain.Common.Audits.Interfaces;

public interface IUserTracking
{
    string CreatedBy { get; set; }
    string? LastModifiedBy { get; set; }
}