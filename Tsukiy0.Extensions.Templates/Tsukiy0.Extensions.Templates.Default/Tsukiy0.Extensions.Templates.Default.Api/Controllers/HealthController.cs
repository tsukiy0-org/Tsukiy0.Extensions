using MediatR;

using Microsoft.AspNetCore.Mvc;

using Tsukiy0.Extensions.Templates.Default.Domain.Requests;

namespace Tsukiy0.Extensions.Templates.Default.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly IMediator _mediator;

    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Health()
    {
        await _mediator.Send(new HealthRequest());
        return Ok();
    }
}
