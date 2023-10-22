using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers.VersionOne;


public class AuthController : BaseController
{
    [AllowAnonymous]
    [HttpGet("shop-owner-information")]
    public async Task<IActionResult> GetAsync()
    {
        var result = new ApiSimpleResult()
        {
            Data = new
            {
                FullName = "Đỗ Chí Hùng",
                DateOfBirth = new DateTime(2002, 09, 06),
                Phone = "0976580418",
                Email = "dohung.csharp@gmail.com",
                Facebook = "https://www.facebook.com/dohungiy",
                MostBeautifulDay = "Ngày em đẹp nhất là ngày anh chưa có gì trong tay!"
            }
        };
        
        return Ok(result);
    }
    

    #region [Ping]

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new ApiSimpleResult("pong pong pong"));
    }

    #endregion

    #region [SignIn + SignOut]

    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpPost("sign-in-by-phone")]
    public async Task<IActionResult> SignInByPhoneAsync(SignInByPhoneCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpPost("sign-in-by-facebook")]
    public async Task<IActionResult> SignInByFacebookAsync(SignInByFacebookCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpPost("sign-in-by-google")]
    public async Task<IActionResult> SignInByGoogleAsync(SignInByGoogleCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("sign-out")]
    public async Task<IActionResult> SignOut(CancellationToken cancellationToken = default)
    {
        await Mediator.Send(new SignOutCommand(), cancellationToken);
        return Ok(new ApiResult());
    }

    [HttpGet("revoke")]
    public async Task<IActionResult> SignOutAllDeviceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new SignOutAllDeviceCommand(userId), cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    #endregion

    #region [SignUp]

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    
    #endregion
    
    [HttpGet("history-paging")]
    public async Task<IActionResult> SignInHistoryPagingAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new SignInHistoryPagingQuery(new PagingRequest(page, size)), cancellationToken);
        return Ok(result);
    }

    #region [Request Information]

    [Authorize]
    [HttpGet("request-information")]
    public async Task<IActionResult> GetRequestInformationAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetRequestInformationQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("request-and-user-information")]
    public async Task<IActionResult> GetRequestAndUserInformationAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetRequestAndUserInformationQuery();
        var result = await Mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    #endregion
}