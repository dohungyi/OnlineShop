using SharedKernel.Auth;
using SharedKernel.Libraries.Utility;

namespace SharedKernel.Domain;

public class AuditEvent : DomainEvent
{
    public AuditEvent(string tableName, AuditAction auditAction, ICurrentUser token, Guid eventId = default) : base(eventId, null, token)
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
        if(token.Context.HttpContext != null)
        {
            IpAddress = AuthUtility.TryGetIP(token.Context.HttpContext.Request);
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