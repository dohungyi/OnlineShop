namespace SharedKernel.Domain;

public class BaseLocation : BaseEntity
{
    public string Name { get; set; }

    public LocationType Type { get; set; }
}