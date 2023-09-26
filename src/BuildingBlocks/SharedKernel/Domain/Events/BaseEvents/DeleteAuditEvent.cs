using SharedKernel.Auth;

namespace SharedKernel.Domain;

public class DeleteAuditEvent<T> : AuditEvent where T : IBaseEntity
{
    public List<T> Entities { get; }

    public DeleteAuditEvent(List<T> entities, ICurrentUser currentUser) : base(typeof(T).Name, AuditAction.Delete, currentUser)
    {
        Entities = entities;
    }
}