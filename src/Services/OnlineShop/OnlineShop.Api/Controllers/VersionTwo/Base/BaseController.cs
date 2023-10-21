using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers.VersionTwo;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class BaseController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    
}