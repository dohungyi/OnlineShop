using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application;
using SharedKernel.Properties;

namespace SharedKernel.Middlewares;

public class UnauthorizedHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Request
        // ....

        await _next(context);
        
        // Response
        
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";

            var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();
            
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult() 
                { Status = "unauthorized",Error = new Error(401, localizer["unauthorized"])}, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                
            }));
        }
    }
}

public static class UnauthorizedHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseCoreUnauthorized(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UnauthorizedHandlerMiddleware>();
    }
}