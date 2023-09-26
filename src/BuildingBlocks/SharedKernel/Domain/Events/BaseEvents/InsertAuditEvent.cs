using SharedKernel.Auth;

namespace SharedKernel.Domain;

public class InsertAuditEvent<T> : AuditEvent where T : IBaseEntity
{
    public List<T> Entities { get; set; }

    public InsertAuditEvent(List<T> entities, ICurrentUser currentUser) : base(typeof(T).Name, AuditAction.Insert, currentUser)
    {
        Entities = entities;
    }
}