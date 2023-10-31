using SharedKernel.Domain;

namespace OnlineShop.Audit.Models;

public class UpdateAuditModel<T> where T : IBaseEntity
{
    public T NewValue { get; set; }

    public T OldValue { get; set; }

    public UpdateAuditModel(T newValue, T oldValue)
    {
        NewValue = newValue;
        OldValue = oldValue;
    }
}