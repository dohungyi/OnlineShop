using MediatR;
using SharedKernel.Application;
using SharedKernel.Auth;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;

namespace SharedKernel.Infrastructures;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUser _currentUser;
    private readonly IAuthService _authService;

    public AuthorizationBehavior(ICurrentUser currentUser, IAuthService authService)
    {
        _currentUser = currentUser;
        _authService = authService;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is BaseQuery<TResponse> || request is BaseCommand<TResponse> || request is BaseCommand)
        {
            var attribute = (AuthorizationRequestAttribute)request.GetType().GetCustomAttributes(typeof(AuthorizationRequestAttribute), false).FirstOrDefault();
            if (attribute is not null)
            {
                var allowAnonymous = attribute.Exponents.Contains(ActionExponent.AllowAnonymous);
                if (!allowAnonymous)
                {
                    var exponents = attribute.Exponents;
                    if (request is BaseInsertCommand<TResponse> || request is BaseInsertCommand)
                    {
                        exponents = exponents.Append(ActionExponent.Add).ToArray();
                    }
                    else if (request is BaseUpdateCommand<TResponse> || request is BaseUpdateCommand)
                    {
                        exponents = exponents.Append(ActionExponent.Edit).ToArray();
                    }
                    else if (request is BaseDeleteCommand<TResponse> || request is BaseDeleteCommand)
                    {
                        exponents = exponents.Append(ActionExponent.Delete).ToArray();
                    }

                    var hasPermission = _authService.CheckPermission(exponents);
                    if (!hasPermission)
                    {
                        throw new ForbiddenException();
                    }
                }
            }
        }
        
        return await next();
    }
}