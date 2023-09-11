namespace SharedKernel.Domain;

public interface ICoreEntity
{
    string GetTableName();
    object this[string propertyName] { get; set; }
}