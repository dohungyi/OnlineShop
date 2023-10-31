using SharedKernel.Domain;

namespace OnlineShop.Audit.Events;

public class IntegrationDeleteAuditEvent<T> : IntegrationAuditEvent<T> where T : IBaseEntity
{
    public List<T> Entities { get; set; }
}