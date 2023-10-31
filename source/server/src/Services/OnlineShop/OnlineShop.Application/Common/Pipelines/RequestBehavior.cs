using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SharedKernel.Log;
using UAParser;


namespace OnlineShop.Application.Pipelines;

public class RequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IServiceProvider _provider;

    public RequestBehavior(IHttpContextAccessor accessor, IServiceProvider provider)
    {
        _accessor = accessor;
        _provider = provider;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestId = Guid.NewGuid().ToString();
        var httpRequest = _accessor.HttpContext.Request;
        var openRequest = new OpenHttpRequest(httpRequest.Method, httpRequest.Scheme, httpRequest.Host,
            httpRequest.Path, httpRequest.QueryString, httpRequest.Headers, AuthUtility.TryGetIP(httpRequest));

        httpRequest.Headers.Add(HeaderNamesExtension.RequestId, requestId);
        
        _ = Task.Run(async () =>
        {
            using (var scope = _provider.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var requestInformation = GetParameter(openRequest, requestId);
                    await context.RequestInformations.AddAsync(requestInformation, cancellationToken);
                    await context.CommitAsync(false ,cancellationToken);
                }
                catch (Exception ex)
                {
                    Logging.Error(ex);
                }
            } 
            
        }, cancellationToken);
        
        return await next();
    }

    private RequestInformation GetParameter(OpenHttpRequest request, string requestId)
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

        return new RequestInformation(requestId, ip, method, apiUrl, browser, os, device, ua, origin);
    }
    
}