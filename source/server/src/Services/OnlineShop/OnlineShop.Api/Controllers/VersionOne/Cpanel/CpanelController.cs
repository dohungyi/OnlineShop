using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers.VersionOne;

public class CpanelController : BaseController
{
    [HttpGet("role/roles")]
    public async Task<IActionResult> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetRolesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("dashboard/total-records")]
    public async Task<IActionResult> GetRecordDashboardAsync(CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetRecordDashboardQuery(), cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUserAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
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