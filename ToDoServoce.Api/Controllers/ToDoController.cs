using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoService.Application.Commands;
using ToDoService.Application.Queries;
using ToDoServoce.Api.Dtos;

namespace ToDoServoce.Api.Controllers;

[ApiController]
[Route("api/todos")]
[Authorize]
public class ToDoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ToDoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        var todos = await _mediator.Send(new GetToDoQuery(userId));
        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateToDoDto dto)
    {
        var userId = GetUserId();
        var command = new CreateToDoCommand(
            dto.Title,
            userId,
            dto.DueDate,
            dto.Priority
        );
        
        var todo = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateToDoDto dto)
    {
        var userId = GetUserId();
        var command = new UpdateToDoCommand(
            id,
            userId,
            dto.Title,
            dto.IsCompleted,
            dto.DueDate,
            dto.Priority
        );
        
        try
        {
            var todo = await _mediator.Send(command);
            return Ok(todo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> MarkAsCompleted(int id)
    {
        var userId = GetUserId();
        var command = new UpdateToDoCommand(id, userId, IsCompleted: true);

        try
        {
            var todo = await _mediator.Send(command);
            return Ok(todo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var deleted = await _mediator.Send(new DeleteToDoCommand(id, userId));
        
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}