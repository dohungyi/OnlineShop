using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Ocelot.Infrastructure.Extensions;
using SharedKernel.Application.Consts;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Libraries.Utility;

namespace SharedKernel.Filters;

public class AccessTokenValidatorAsyncFilter : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        if (AuthUtility.EndpointRequiresAuthorize(context))
        {
            var bearerToken = context.HttpContext.Request.Headers[HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(bearerToken.GetValue()))
            {
                var token = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
                var sequenceCaching = context.HttpContext.RequestServices.GetRequiredService<ISequenceCaching>();
                var accessToken = bearerToken.GetValue()[7..];
                var key = BaseCacheKeys.GetRevokeAccessTokenKey(accessToken);
                var isRevoked = !string.IsNullOrEmpty(await sequenceCaching.GetStringAsync(key));

                // Nếu người dùng đã đăng xuất
                if (isRevoked || string.IsNullOrEmpty(token.Context.UserId))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        ContentType = "application/json"
                    };
                    return;
                }
            }
        }
        await next();
    }
}