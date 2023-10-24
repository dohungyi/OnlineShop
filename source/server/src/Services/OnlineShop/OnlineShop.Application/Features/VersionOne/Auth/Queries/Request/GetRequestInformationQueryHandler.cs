
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SharedKernel.Libraries;
using SharedKernel.Libraries.Utility;
using UAParser;

namespace OnlineShop.Application.Features.VersionOne;

public class GetRequestInformationQueryHandler : IRequestHandler<GetRequestInformationQuery, ApiResult>
{
    private readonly IHttpContextAccessor _context;
    private readonly IServiceProvider _provider;
    public GetRequestInformationQueryHandler(
        IHttpContextAccessor context,
        IServiceProvider provider
        )
    {
        _context = context;
        _provider = provider;
    }

    public async Task<ApiResult> Handle(GetRequestInformationQuery request, CancellationToken cancellationToken)
    {
        var value = new RequestValue();
        var httpRequest = _context.HttpContext.Request;
        var header = httpRequest.Headers;
        var ua = header[HeaderNames.UserAgent].ToString();
        var c = Parser.GetDefault().Parse(ua);

        value.Ip = AuthUtility.TryGetIP(httpRequest);
        value.UA = ua;
        value.OS = c.OS.Family + (!string.IsNullOrEmpty(c.OS.Major) ? $" {c.OS.Major}" : "") + (!string.IsNullOrEmpty(c.OS.Minor) ? $".{c.OS.Minor}" : "");
        value.Browser = c.UA.Family + (!string.IsNullOrEmpty(c.UA.Major) ? $" {c.UA.Major}.{c.UA.Minor}" : "");
        value.Device = c.Device.Family;
        value.Origin = header[HeaderNames.Origin];
        value.Time = DateHelper.Now.ToString();
        value.IpInformation = await AuthUtility.GetIpInformationAsync(_provider, value.Ip);

        var result = new ApiSimpleResult(value);
        
        return result;
    }
}