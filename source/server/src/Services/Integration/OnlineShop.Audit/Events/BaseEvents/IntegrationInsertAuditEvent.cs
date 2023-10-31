using SharedKernel.Domain;

namespace OnlineShop.Audit.Events;

public class IntegrationInsertAuditEvent<T> : IntegrationAuditEvent<T> where T : IBaseEntity
{
    public List<T> Entities { get; set; }
}