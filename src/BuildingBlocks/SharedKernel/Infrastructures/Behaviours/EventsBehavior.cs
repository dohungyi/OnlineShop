// using MediatR;
// using SharedKernel.Domain;
// using SharedKernel.Persistence;
//
// namespace SharedKernel.Infrastructures;
//
// public class EventsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
// {
//     private readonly IAppDbContext _context;
//     private readonly IEventBus _eventBus;
//
//     public EventsBehavior(IAppDbContext context, IEventBus eventBus)
//     {
//         _context = context ?? throw new ArgumentNullException(nameof(context));
//         _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
//     }
//     
//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//     {
//         var result = await next();
//         
//         await _context.PublishEvents(_eventBus, cancellationToken);
//
//         return result;
//     }
//     
// }