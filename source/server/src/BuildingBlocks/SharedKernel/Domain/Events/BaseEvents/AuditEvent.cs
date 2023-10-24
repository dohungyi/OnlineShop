using SharedKernel.Auth;
using SharedKernel.Libraries.Utility;

namespace SharedKernel.Domain;

public class AuditEvent : DomainEvent
{
    public AuditEvent(string tableName, AuditAction auditAction, ICurrentUser currentUser, Guid eventId = default) : base(eventId, null, currentUser)
    {
        TableName = tableName;
        AuditAction = auditAction;
        if (eventId == Guid.Empty)
        {
            EventId = Guid.NewGuid();
        }
        if (string.IsNullOrEmpty(EventQueue))
        {
            EventQueue = "audit-event";
        }
        if(currentUser.Context.HttpContext != null)
        {
            IpAddress = AuthUtility.TryGetIP(currentUser.Context.HttpContext.Request);
        }
        else
        {
            IpAddress = "";
        }
    }

    public string TableName { get; protected set; }

    public AuditAction AuditAction { get; protected set; }

    public string IpAddress { get; protected set; }
}