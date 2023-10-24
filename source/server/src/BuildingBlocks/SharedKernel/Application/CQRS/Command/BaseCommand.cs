using MediatR;

namespace SharedKernel.Application;

public abstract class BaseCommand<TResponse> : IRequest<TResponse>
{
    
}

public abstract class BaseCommand : IRequest
{
    
}