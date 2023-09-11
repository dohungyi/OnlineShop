using SharedKernel.Auth;

namespace SharedKernel.Domain;

public class InsertAuditEvent<T> : AuditEvent where T : IBaseEntity
{
    public List<T> Entities { get; set; }

    public InsertAuditEvent(List<T> entities, ICurrentUser token) : base(typeof(T).Name, AuditAction.Insert, token)
    {
        Entities = entities;
    }
}