namespace SharedKernel.Domain;

public interface IPersonalizeEntity : IBaseEntity
{
    Guid OwnerId { get; set; }
}