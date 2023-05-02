using AiTelegramChannel.ServerHost.Components.GetFuturePublications;
using AiTelegramChannel.ServerHost.Components.GetFuturePublications.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AiTelegramChannel.ServerHost.Controllers;

[ApiController]
[Route("publications")]
public class PublicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetFuturePublicationsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetFuturePublicationsRequest());
        return Ok(result.Value);
    }
}