using OnlineShop.Audit.Events;
using OnlineShop.Audit.Models;
using OnlineShop.Audit.Persistence;
using SharedKernel.Domain;

namespace OnlineShop.Audit.Processes;

public class SignInProcess : BaseProcess<BaseEntity>
{

    public SignInProcess(IntegrationAuditDbContext dbContext, AuditConfigModel config = default) : base(dbContext, config)
    {
        
    }

    protected override List<AuditEntity> GetParameter(IntegrationAuditEvent<BaseEntity> auditEvent, string bodyStr)
    {
        return new List<AuditEntity> { CreateBaseAuditEntity(auditEvent, "<p>Đăng nhập thành công</p>") };
    }
    
}