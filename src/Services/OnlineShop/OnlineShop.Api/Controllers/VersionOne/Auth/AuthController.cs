using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Features.VersionOne;
using SharedKernel.Application.Responses;

namespace OnlineShop.Api.Controllers;


public class AuthController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(new { Name = "Đỗ Chí Hùng" });
    }
    
    [HttpGet("request-information"), AllowAnonymous]
    public async Task<IActionResult> RequestInformation(CancellationToken cancellationToken = default)
    {
        var query = new GetRequestInformationQuery();
        var result = new ApiSuccessResult<object>() { Data = await Mediator.Send(query, cancellationToken) };
        return Ok(result);
    }
}