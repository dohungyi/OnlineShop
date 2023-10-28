
namespace OnlineShop.Application.Pipelines;

public class EventsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;

    public EventsBehavior(IApplicationDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }
    

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next();
        
        await _context.PublishEvents(_eventBus, cancellationToken);

        return result;
    }
}