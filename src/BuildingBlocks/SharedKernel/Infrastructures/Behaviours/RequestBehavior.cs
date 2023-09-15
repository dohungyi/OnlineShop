using System.Data.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SharedKernel.Application;
using SharedKernel.Domain;
using SharedKernel.Libraries.Utility;
using SharedKernel.Log;
using UAParser;

namespace SharedKernel.Infrastructures;

public class RequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _context;

    public RequestBehavior(IHttpContextAccessor accessor)
    {
        _context = accessor;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var requestId = Guid.NewGuid().ToString();
        var httpRequest = _context.HttpContext.Request;
        var openRequest = new OpenHttpRequest(httpRequest.Method, httpRequest.Scheme, httpRequest.Host,
            httpRequest.Path, httpRequest.QueryString, httpRequest.Headers, AuthUtility.TryGetIP(httpRequest));

        httpRequest.Headers.Add(HeaderNamesExtension.RequestId, requestId);
        _ = Task.Run(async () =>
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logging.Error(ex);
            }
        });
        return await next();
    }

    private (string, RequestInformation) GetParameter(OpenHttpRequest request, string requestId)
    {
        var ua = request.Headers[HeaderNames.UserAgent].ToString();
        var c = Parser.GetDefault().Parse(ua);
        var method = request.Method;
        var origin = request.Headers[HeaderNames.Origin];
        var device = c.Device.Family;
        var apiUrl = $"{request.Scheme}://{request.Host}{request.Path.Value}{request.QueryString.Value}";
        var ip = request.Ip;
        var browser = c.UA.Family + (!string.IsNullOrEmpty(c.UA.Major) ? $" {c.UA.Major}.{c.UA.Minor}" : "");
        var os = c.OS.Family + (!string.IsNullOrEmpty(c.OS.Major) ? $" {c.OS.Major}" : "") +
                 (!string.IsNullOrEmpty(c.OS.Minor) ? $".{c.OS.Minor}" : "");

        return (default, default);
    }
}