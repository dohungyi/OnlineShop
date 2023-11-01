namespace SharedKernel.Domain;

public interface ICoreEntity
{
    object this[string propertyName] { get; set; }
}