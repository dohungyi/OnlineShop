
namespace OnlineShop.Application.Pipelines;

public class EventsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventDispatcher _eventDispatcher;

    public EventsBehavior(IApplicationDbContext context, IEventDispatcher eventDispatcher)
    {
        _context = context;
        _eventDispatcher = eventDispatcher;
    }
    

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next();
        
        await _context.PublishEvents(_eventDispatcher, cancellationToken);

        return result;
    }
}