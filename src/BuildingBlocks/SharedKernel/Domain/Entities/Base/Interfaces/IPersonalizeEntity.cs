namespace SharedKernel.Domain;

public interface IPersonalizeEntity : IBaseEntity
{
    Guid UserId { get; set; }
}