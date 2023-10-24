using MediatR;

namespace SharedKernel.Domain;

public static class MediatorExtension
{
    public static Task PublishDomainEventsAsync(this IMediator mediator, IList<DomainEvent> events, CancellationToken cancellationToken = default)
    {
        if (events is not null && events.Any())
        {
            foreach (var @event in events)
            {
                mediator.Publish(@event, cancellationToken);
            }
        }
        return Task.CompletedTask;  
    }
}