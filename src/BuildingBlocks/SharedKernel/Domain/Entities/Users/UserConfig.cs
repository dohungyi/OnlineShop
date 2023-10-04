namespace SharedKernel.Domain;

public class UserConfig : PersonalizedEntity
{
    public string Json { get; set; }
    
    public Guid UserId { get; set; }
}