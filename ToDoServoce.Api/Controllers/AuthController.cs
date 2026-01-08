using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoService.Application.Commands;
using ToDoServoce.Api.Dtos;

namespace ToDoServoce.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var command = new RegisterCommand(dto.Username, dto.Email, dto.Password);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var command = new LoginCommand(dto.Username, dto.Password);
        var result = await _mediator.Send(command);

        if (result == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(result);
    }
}
