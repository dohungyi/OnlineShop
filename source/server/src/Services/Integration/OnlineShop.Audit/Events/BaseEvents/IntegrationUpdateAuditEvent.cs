using SharedKernel.Domain;

namespace OnlineShop.Audit.Events;

public class IntegrationUpdateAuditEvent<T> : IntegrationAuditEvent<T> where T : IBaseEntity
{
    
}