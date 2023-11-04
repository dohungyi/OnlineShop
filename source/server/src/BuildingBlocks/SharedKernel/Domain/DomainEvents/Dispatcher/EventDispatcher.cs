using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Libraries;
using SharedKernel.Log;
using SharedKernel.MessageBroker;

namespace SharedKernel.Domain.DomainEvents.Dispatcher;

public class EventDispatcher : IEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _mediator = _serviceProvider.GetRequiredService<IMediator>();
        _messagePublisher = _serviceProvider.GetRequiredService<IMessagePublisher>();
        _configuration = _serviceProvider.GetRequiredService<IConfiguration>();
    }
    
    public async Task PublishEvent<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        await Task.Yield();
        Logging.Information($"[{@event.EventType}] Fire event [{@event.EventId}] at {@event.Timestamp}");

        @event.CurrentUser.Context.HttpContext = null;
        _ = Publish(@event, cancellationToken);
    }

    public async Task PublishEvent<T>(List<T> events, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        await Task.Yield();
        foreach (var @event in events)
        {
            _ = PublishEvent(@event, cancellationToken);
        }
    }

    #region [PRIVATE METHODS]

    private async Task Publish<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        var mediatorTask = _mediator.Publish(@event, cancellationToken);
        var saveTask = SaveEventAsync(@event, cancellationToken);
        var mqTask = _messagePublisher.PublishAsync(@event, cancellationToken: cancellationToken);

        await Task.WhenAll(mediatorTask, saveTask, mqTask);
    }
    
    private async Task SaveEventAsync<T>(T @event, CancellationToken cancellationToken) where T : DomainEvent
    {
        try
        {
            using (var context = new EventDbContext())
            {
                var eventNew = new Event
                {
                    EventId = @event.EventId.ToString(),
                    EventType = @event.EventType,
                    Timestamp = @event.Timestamp,
                    Body = JsonConvert.SerializeObject(@event.Body),
                    CreatedDate = DateHelper.Now
                };
                
                await context.Events.AddAsync(eventNew, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Logging.Error(ex);
            throw;
        }
    }

    #endregion
}