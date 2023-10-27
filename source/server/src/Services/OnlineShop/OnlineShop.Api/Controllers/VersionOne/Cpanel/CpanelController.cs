using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Features.VersionOne.Cpanel.CreateUser;

namespace OnlineShop.Api.Controllers.VersionOne;

public class CpanelController : BaseController
{
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        var uid = await Mediator.Send(request, cancellationToken);
        return Ok(new ApiSimpleResult() { Data = uid });
    }
}