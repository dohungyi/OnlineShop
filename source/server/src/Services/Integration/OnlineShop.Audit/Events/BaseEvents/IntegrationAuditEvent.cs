using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Libraries;

namespace OnlineShop.Audit.Events;

public class IntegrationAuditEvent<T> where T : IBaseEntity
{
    public string TableName { get; set; }

    public AuditAction AuditAction { get; set; }

    public List<UpdateAuditModel<T>> UpdateModels { get; set; }
    
    public Guid EventId { get; set; }

    public DateTime Timestamp { get; set; } = DateHelper.Now;

    public string EventType { get; set; }

    public string EventQueue { get; set; }

    public ICurrentUser CurrentUser { get; set; }

    public string IpAddress { get; set; }

    public IntegrationAuditEvent()
    {
        EventQueue = "audit-event";
    }
}