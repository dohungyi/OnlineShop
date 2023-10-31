using System.Reflection;
using OnlineShop.Audit.Events;
using OnlineShop.Audit.Models;
using SharedKernel.Domain;

namespace OnlineShop.Audit.Processes;

public class BaseProcess<T> where T : IBaseEntity
{
    protected readonly AuditConfigModel _config;
    protected readonly IEnumerable<PropertyInfo> _properties;

    public BaseProcess(AuditConfigModel config = default)
    {
        _config = config ?? new AuditConfigModel();
        _properties = typeof(T).GetProperties().Where(p => p.GetIndexParameters().Length == 0);
    }

    #region [Get parameters]

    protected virtual List<AuditEntity> GetParameter(IntegrationAuditEvent<T> auditEvent, string bodyStr)
    {
        var ignoreFields = new string[]
        {
            nameof(BaseEntity.DomainEvents),
            nameof(BaseEntity.Id),
            nameof(BaseEntity.LastModifiedDate),
            nameof(BaseEntity.LastModifiedBy),
            nameof(BaseEntity.CreatedDate),
            nameof(BaseEntity.CreatedBy),
            nameof(BaseEntity.DeletedDate),
            nameof(BaseEntity.DeletedBy),
            nameof(BaseEntity.IsDeleted),
            nameof(PersonalizedEntity.UserId),
        };
        
        switch (auditEvent.AuditAction)
        {
            case AuditAction.Insert:
            {
                return GetInsertParameter(bodyStr, ignoreFields);
            }
            case AuditAction.Update:
            {
                return GetUpdateParameter(bodyStr, ignoreFields);
            }
            case AuditAction.Delete:
            {
                return GetDeleteParameter(bodyStr, ignoreFields);
            }
            default:
            {
                return GetCustomParameter(auditEvent, bodyStr, ignoreFields);
            }
        }
    }

    private List<AuditEntity> GetCustomParameter(IntegrationAuditEvent<T> auditEvent, string bodyStr, string[] ignoreFields)
    {
        throw new NotImplementedException();
    }

    private List<AuditEntity> GetDeleteParameter(string bodyStr, string[] ignoreFields)
    {
        throw new NotImplementedException();
    }

    private List<AuditEntity> GetUpdateParameter(string bodyStr, string[] ignoreFields)
    {
        throw new NotImplementedException();
    }

    private List<AuditEntity> GetInsertParameter(string bodyStr, string[] ignoreFields)
    {
        throw new NotImplementedException();
    }

    protected AuditEntity CreateBaseAuditEntity(IntegrationAuditEvent<T> @event, string description)
    {
        var audit = new AuditEntity();
        audit.Action = (int)@event.AuditAction;
        audit.TableName = @event.TableName;
        audit.Timestamp = @event.Timestamp;
        audit.CreatedBy = @event.CurrentUser.Context.UserId;
        audit.IpAddress = @event.IpAddress ?? "";
        audit.Description = description;

        return audit;
    }
    
    #endregion
}