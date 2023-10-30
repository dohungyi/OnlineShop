using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.VersionOne.Config.Queries;

namespace OnlineShop.Api.Controllers.VersionOne.Config;

public class ConfigController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        return Ok(await Mediator.Send(new GetConfigQuery(), cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> CreateOrUpdateAsync(ConfigValue configValue, CancellationToken cancellationToken = default)
    {
        var command = new CreateOrUpdateConfigCommand(configValue);
        return Ok(await Mediator.Send(command, cancellationToken));
    }
}