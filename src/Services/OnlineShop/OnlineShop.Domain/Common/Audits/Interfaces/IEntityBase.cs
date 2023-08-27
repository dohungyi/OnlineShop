namespace OnlineShop.Domain.Common.Audits.Interfaces;

public interface IEntityBase<T>
{
    T Id { get; set; }   
}