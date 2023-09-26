using SharedKernel.Auth;

namespace SharedKernel.Domain;

public class UpdateAuditEvent<T> : AuditEvent where T : IBaseEntity
{
    public List<UpdateAuditModel<T>> UpdateModels { get; }

    public UpdateAuditEvent(List<UpdateAuditModel<T>> updateModels, ICurrentUser currentUser) : base(typeof(T).Name, AuditAction.Update, currentUser)
    {
        UpdateModels = updateModels;
    }
}