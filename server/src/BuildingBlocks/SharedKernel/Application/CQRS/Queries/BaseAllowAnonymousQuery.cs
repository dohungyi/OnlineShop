using SharedKernel.Libraries;

namespace SharedKernel.Application;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class BaseAllowAnonymousQuery<TResponse> : BaseQuery<TResponse>
{
}