using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Features.VersionOne.Cpanel.CreateUser;
using OnlineShop.Application.Features.VersionOne.Cpanel.Queries.GetUserPaging;

namespace OnlineShop.Api.Controllers.VersionOne;

public class CpanelController : BaseController
{
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        var uid = await Mediator.Send(request, cancellationToken);
        return Ok(new ApiSimpleResult() { Data = uid });
    }

    [HttpGet("user/users")]
    public async Task<IActionResult> GetUsersAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetUserPagingQuery(new PagingRequest(page, size)), cancellationToken);

        return Ok(result);
    }
    
}