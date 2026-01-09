using MediatR;
using ToDoService.Application.Commands;
using ToDoService.Application.Interfaces;
using ToDoService.Domain.Entities;

namespace ToDoService.Application.Handlers;

public class UpdateToDoHandler : IRequestHandler<UpdateToDoCommand, ToDoItem>
{
    private readonly IToDoRepository _repository;

    public UpdateToDoHandler(IToDoRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ToDoItem> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetByIdAsync(request.Id);

        if (todo == null)
        {
            throw new KeyNotFoundException("Item not found. ID=" + request.Id);
        }

        if (todo.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You don't have permission to update this item");
        }

        if (request.Title != null)
        {
            todo.Title = request.Title;
        }

        if (request.IsCompleted.HasValue)
        {
            todo.IsCompleted = request.IsCompleted.Value;
        }

        if (request.DueDate.HasValue)
        {
            var dueDate = request.DueDate.Value;
            if (dueDate.Kind == DateTimeKind.Unspecified)
            {
                todo.DueDate = DateTime.SpecifyKind(dueDate, DateTimeKind.Utc);
            }
            else
            {
                todo.DueDate = dueDate.ToUniversalTime();
            }
        }

        if (request.Priority.HasValue)
        {
            todo.Priority = request.Priority.Value;
        }

        await _repository.UpdateAsync(todo);

        return todo;
    }
}