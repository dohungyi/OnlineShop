using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OnlineShop.Application.Infrastructure;
using OnlineShop.Application.Services;
using SharedKernel.Libraries;
using SharedKernel.Libraries.Utility;
using UAParser;

namespace OnlineShop.Application.Features.VersionOne;

public class GetRequestAndUserInformationQueryHandler
    : IRequestHandler<GetRequestAndUserInformationQuery, ApiResult>
{
    private readonly IHttpContextAccessor _context;
    private readonly IServiceProvider _provider;
    private readonly ICurrentUser _currentUser;
    private readonly IAuthRepository _authRepository;

    public GetRequestAndUserInformationQueryHandler(
        IHttpContextAccessor context,
        IServiceProvider provider,
        ICurrentUser currentUser,
        IAuthRepository authRepository)
    {
        (_context, _provider, _currentUser, _authRepository)
            = (context, provider, currentUser, authRepository);
    }
    
    public async Task<ApiResult> Handle(GetRequestAndUserInformationQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_currentUser.Context.UserId);
        
        var requestValueTask =  GetRequestValue();
        
        var userInformationTask =  _authRepository.GetTokenUserByIdAsync(userId, cancellationToken);
        
        await Task.WhenAll(requestValueTask, userInformationTask);

        return new ApiSimpleResult()
        {
            Data = new
            {
                UserInfo = await userInformationTask,
                RequestInfo = await requestValueTask
            }
        };
    }

    private async Task<RequestValue> GetRequestValue()
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

        return value;
    }
}