using AiTelegramChannel.ServerHost.Components.GetToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AiTelegramChannel.ServerHost.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-token")]
    public async Task<IActionResult> GetToken([FromQuery] GetTokenRequest request)
    {
        var result = await _mediator.Send(request);
        
        if (result.IsFailed)
        {
            return Unauthorized(result.Errors);
        }

        return Ok(result.Value);
    }
}