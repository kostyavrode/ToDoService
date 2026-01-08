using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoService.Application.Commands;
using ToDoService.Application.Interfaces;
using ToDoService.Application.Queries;
using ToDoServoce.Api.Dtos;

namespace ToDoServoce.Api.Controllers;

[ApiController]
[Route("api/todos")]
public class ToDoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ToDoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //var todos = await _service.GetAllAsync();
        var todos = await _mediator.Send(new GetToDoQuery());
        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateToDoCommand createToDoCommand)
    {
        if (string.IsNullOrWhiteSpace(createToDoCommand.Title))
            return BadRequest("Title is required");

        //var todo = await _service.CreateAsync(dto.Title);
        var todo = await _mediator.Send(createToDoCommand);
        return Ok(todo);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        //var deleted = await _service.DeleteAsync(id);
        var deleted = await _mediator.Send(new DeleteToDoCommand(id));
        
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> MarkAsCompleted(int id)
    {
        var command = new UpdateToDoCommand(id, IsCompleted: true);

        try
        {
            var todo = await _mediator.Send(command);
            return Ok(todo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}