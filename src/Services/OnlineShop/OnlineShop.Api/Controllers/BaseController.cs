using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}