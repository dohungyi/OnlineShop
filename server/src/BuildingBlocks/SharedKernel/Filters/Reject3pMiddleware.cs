using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application;
using SharedKernel.Core;
using SharedKernel.Libraries.Utility;
using SharedKernel.Properties;

namespace SharedKernel.Filters;

public class Reject3pMiddleware
{
    private readonly RequestDelegate _next;

    public Reject3pMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var header = context.Request.Headers;
        var pass3p = !string.IsNullOrEmpty(header[HeaderNamesExtension.Pass3p]);
        if (!pass3p)
        {
            // handler
        }
        await _next(context);
    }
}

public static class Reject3pMiddlewareExtension
{
    public static IApplicationBuilder UseReject3P(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Reject3pMiddleware>();
    }
}