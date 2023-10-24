namespace SharedKernel.Domain;

public abstract class PersonalizedEntity : BaseEntity, IPersonalizeEntity
{
    public Guid UserId { get; set; }
}