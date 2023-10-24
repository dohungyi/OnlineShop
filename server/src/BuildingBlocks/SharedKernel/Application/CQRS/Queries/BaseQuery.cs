using MediatR;
using SharedKernel.Libraries;

namespace SharedKernel.Application;

[AuthorizationRequest]
public abstract class BaseQuery<TResponse> : IRequest<TResponse>
{
}