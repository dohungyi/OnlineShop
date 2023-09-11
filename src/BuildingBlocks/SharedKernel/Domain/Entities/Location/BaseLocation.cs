namespace SharedKernel.Domain.Entities.Location;

public class BaseLocation : BaseEntity
{
    public string Name { get; set; }

    public LocationType Type { get; set; }
}