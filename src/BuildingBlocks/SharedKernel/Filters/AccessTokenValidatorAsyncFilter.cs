using Microsoft.AspNetCore.Mvc.Filters;

namespace SharedKernel.Filters;

public class AccessTokenValidatorAsyncFilter : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}