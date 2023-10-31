using MassTransit;
using OnlineShop.Audit.Entities;
using OnlineShop.Audit.Events;
using SharedKernel.Domain;
using SharedKernel.Log;

namespace OnlineShop.Audit.Consumers;

public class AuditConsumer : IConsumer<IntegrationAuditEvent<BaseEntity>>
{
    public async Task Consume(ConsumeContext<IntegrationAuditEvent<BaseEntity>> context)
    {
        var @event = context.Message;
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
                // new SignInProcess().HandleAsync(bodyStr).GetAwaiter().GetResult();
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