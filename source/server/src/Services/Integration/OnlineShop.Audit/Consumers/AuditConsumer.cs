using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineShop.Audit.Entities;
using OnlineShop.Audit.Events;
using OnlineShop.Audit.Persistence;
using OnlineShop.Audit.Processes;
using SharedKernel.Domain;
using SharedKernel.Log;

namespace OnlineShop.Audit.Consumers;

public class AuditConsumer : IConsumer<JObject>
{
    private readonly IntegrationAuditDbContext _dbContext;
    public AuditConsumer(IntegrationAuditDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Consume(ConsumeContext<JObject> context)
    {
        var bodyStr = context.Message.ToString();
        var @event = JsonConvert.DeserializeObject<IntegrationAuditEvent<BaseEntity>>(bodyStr);
        Logging.Information($"Received an audit event with event id = {@event.EventId}");

        if (@event is null)
        {
            return;
        }

        switch (@event.TableName)
        {
            case nameof(Product):
            {
                // new AvatarProcess().HandleAsync(bodyStr).GetAwaiter().GetResult();
                break;
            }
            case nameof(Avatar):
            {
                // new AvatarProcess().HandleAsync(bodyStr).GetAwaiter().GetResult();
                break;
            }
            case "SignIn":
            {
                new SignInProcess(_dbContext).HandleAsync(bodyStr).GetAwaiter().GetResult();
                break;
            }
            case "SignOut":
            {
                // new SignOutProcess().HandleAsync(bodyStr).GetAwaiter().GetResult();
                break;
            }
            default:
            {
                Logging.Warning("Not found any handler with name = " + @event.TableName);
                break; 
            }
        }
    }
}