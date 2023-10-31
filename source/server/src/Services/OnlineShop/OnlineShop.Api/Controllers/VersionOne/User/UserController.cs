using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers.VersionOne;

public class UserController : BaseController
{
    [HttpGet("user-information")]
    public async Task<IActionResult> GetUserInformationAsync(CancellationToken cancellationToken = default)
    {
        return Ok(new ApiSimpleResult());
        
    }

    [HttpGet("avatar")]
    public async Task<IActionResult> GetAvatarAsync(CancellationToken cancellationToken = default)
    {
        return Ok(new ApiSimpleResult());
    }

    [HttpPost("set-avatar")]
    public async Task<IActionResult> RemoveAvatarAsync(CancellationToken cancellationToken = default)
    {
        return Ok(new ApiSimpleResult());
    }

    [HttpDelete("remove-avatar")]
    public async Task<IActionResult> RemoveAvatar(CancellationToken cancellationToken = default)
    {
        return Ok(new ApiSimpleResult());
    }
}