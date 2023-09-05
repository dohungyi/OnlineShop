namespace SharedKernel.Domain.Entities.Base.Interfaces;

public interface IAuditable
{
    DateTime CreatedDate { get; set; }
    string CreatedBy { get; set; }
    DateTime? LastModifiedDate { get; set; }
    string? LastModifiedBy { get; set; }
    DateTime? DeletedDate { get; set; }
    string? DeletedBy { get; set; }
}