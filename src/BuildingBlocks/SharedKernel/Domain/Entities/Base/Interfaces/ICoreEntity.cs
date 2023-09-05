namespace SharedKernel.Domain.Entities.Base.Interfaces;

public interface ICoreEntity
{
    string GetTableName();
    object this[string propertyName] { get; set; }
}