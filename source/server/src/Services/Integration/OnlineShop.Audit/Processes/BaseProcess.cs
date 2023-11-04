using System.Reflection;
using EFCore.BulkExtensions;
using Newtonsoft.Json;
using OnlineShop.Audit.Events;
using OnlineShop.Audit.Models;
using OnlineShop.Audit.Persistence;
using SharedKernel.Domain;
using SharedKernel.Domain.DomainEvents;
using SharedKernel.Libraries;
using SharedKernel.Log;

namespace OnlineShop.Audit.Processes;

public class BaseProcess<T> where T : IBaseEntity
{
    protected readonly AuditConfigModel _config;
    protected readonly IEnumerable<PropertyInfo> _properties;
    private readonly IntegrationAuditDbContext _dbContext;

    public BaseProcess(IntegrationAuditDbContext dbContext, AuditConfigModel config = default)
    {
        _config = config ?? new AuditConfigModel();
        _properties = typeof(T).GetProperties().Where(p => p.GetIndexParameters().Length == 0);
        _dbContext = dbContext;
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

    protected virtual List<AuditEntity> GetCustomParameter(IntegrationAuditEvent<T> auditEvent, string bodyStr, string[] ignoreFields)
    {
        return new();
    }

    protected virtual List<AuditEntity> GetDeleteParameter(string bodyStr, string[] ignoreFields)
    {
        var @event = JsonConvert.DeserializeObject<IntegrationDeleteAuditEvent<T>>(bodyStr);
        var result = new List<AuditEntity>();

        foreach (var entity in @event.Entities)
        {
            var auditEntity = CreateBaseAuditEntity(@event, GetDeleteDescription(entity, ignoreFields));
            auditEntity.OldValue = JsonConvert.SerializeObject(entity);
            
            result.Add(auditEntity);
        }
        
        return result;
    }

    protected virtual List<AuditEntity> GetUpdateParameter(string bodyStr, string[] ignoreFields)
    {
        var @event = JsonConvert.DeserializeObject<IntegrationUpdateAuditEvent<T>>(bodyStr);
        var result = new List<AuditEntity>();

        foreach (var model in @event.UpdateModels)
        {
            var auditEntity = CreateBaseAuditEntity(@event, GetUpdateDescription(model, ignoreFields));
            auditEntity.OldValue = JsonConvert.SerializeObject(model.OldValue);
            auditEntity.NewValue = JsonConvert.SerializeObject(model.NewValue);
            
            result.Add(auditEntity);
        }
        
        return result;
    }

    protected virtual List<AuditEntity> GetInsertParameter(string bodyStr, string[] ignoreFields)
    {
        var @event = JsonConvert.DeserializeObject<IntegrationInsertAuditEvent<T>>(bodyStr);
        var result = new List<AuditEntity>();

        foreach (var entity in @event.Entities)
        {
            var auditEntity = CreateBaseAuditEntity(@event, GetInsertDescription(entity, ignoreFields));
            auditEntity.NewValue = JsonConvert.SerializeObject(entity);
            
            result.Add(auditEntity);
        }
        
        return result;
    }

    protected virtual AuditEntity CreateBaseAuditEntity(IntegrationAuditEvent<T> @event, string description)
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

    #region [Get descriptions]
    protected virtual string GetInsertDescription(T entity, string[] ignoreFields)
    {
        foreach (var property in _properties)
        {
            var auditableAttribute = property.GetCustomAttribute(typeof(AuditableAttribute), true);
            var auditable = property.IsDefined(typeof(AuditableAttribute), true);
            var propertyName = property.Name;

            if (ignoreFields.Contains(propertyName) || !auditable)
            {
                continue;
            }

            if ((auditableAttribute as AuditableAttribute).UseInsert)
            {
                return $"<p>Thêm <strong>{_config.Module}</strong> với <strong>{property.GetDescription().ToLower()}</strong> là <strong>{entity[propertyName]}</strong></p>";
            }
        }
        
        return string.Empty;
    }

    protected virtual string GetUpdateDescription(SharedKernel.Domain.UpdateAuditModel<T> model, string[] ignoreFields)
    {
        var descriptionId = string.Empty;
        var description = string.Empty;

        foreach (var property in _properties)
        {
            var auditable = property.IsDefined(typeof(AuditableAttribute), true);
            var propertyName = property.Name;
            var newValue = model.NewValue[propertyName];
            var oldValue = model.OldValue[propertyName];

            if (ignoreFields.Contains(propertyName) || !auditable)
            {
                continue;
            }

            if (!Equals(newValue, oldValue))
            {
                var readableNewValue = GetHumanReadableValue(newValue);
                var readableOldValue = GetHumanReadableValue(oldValue);
                if (string.IsNullOrEmpty(descriptionId))
                {
                    descriptionId = $"<p>Bản ghi có id <strong>{model.NewValue.Id}</strong> thay đổi: </p>";
                }
                description += $"<p> - {property.GetDescription()} từ <strong>{readableOldValue}</strong> thành <strong>{readableNewValue}</strong></p>";
            }
        }
        return descriptionId + description;
    }
    
    protected virtual string GetDeleteDescription(T entity, string[] ignoreFields)
    {
        foreach (var property in _properties)
        {
            var propertyName = property.Name;
            var auditableAttribute = property.GetCustomAttribute(typeof(AuditableAttribute), true);
            var auditable = property.IsDefined(typeof(AuditableAttribute), true);

            if (ignoreFields.Contains(propertyName) || !auditable)
            {
                continue;
            }

            if ((auditableAttribute as AuditableAttribute).UseDelete)
            {
                return $"<p>Xóa <strong>{_config.Module}</strong> có id <strong>{entity.Id}</strong> với <strong>{property.GetDescription().ToLower()}</strong> là <strong>{entity[propertyName]}</strong></p>";
            }
        }
        
        return string.Empty;
    }
    
    protected virtual string GetHumanReadableValue (object value)
    {
        if (value?.GetType() == typeof(bool))
        {
            return (bool)value ? "có" : "không";
        }
        return value?.ToString().StripHtml();
    }
    #endregion

    protected virtual async Task SaveAsync(List<AuditEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkInsertEntitiesAsync(entities, cancellationToken);
        await _dbContext.BulkSaveChangesAsync(cancellationToken : cancellationToken);
    }

    public virtual async Task HandleAsync(string bodyStr, CancellationToken cancellationToken = default)
    {
        var auditEvent = JsonConvert.DeserializeObject<IntegrationAuditEvent<T>>(bodyStr);
        var param = GetParameter(auditEvent, bodyStr);
        if (param.Any())
        {
            await SaveAsync(param, cancellationToken);
        }
        else
        {
            Logging.Warning($"{auditEvent.TableName} could not create parameter with event id = {auditEvent.EventId}");
        }
    }
}