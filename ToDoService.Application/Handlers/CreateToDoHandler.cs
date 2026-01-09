using MediatR;
using ToDoService.Application.Commands;
using ToDoService.Application.Interfaces;
using ToDoService.Domain.Entities;

namespace ToDoService.Application.Handlers;

public class CreateToDoHandler : IRequestHandler<CreateToDoCommand, ToDoItem>
{
    private readonly IToDoRepository _repository;

    public CreateToDoHandler(IToDoRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ToDoItem> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
    {
        DateTime? dueDateUtc = null;
        if (request.DueDate.HasValue)
        {
            var dueDate = request.DueDate.Value;
            if (dueDate.Kind == DateTimeKind.Unspecified)
            {
                dueDateUtc = DateTime.SpecifyKind(dueDate, DateTimeKind.Utc);
            }
            else
            {
                dueDateUtc = dueDate.ToUniversalTime();
            }
        }

        var todo = new ToDoItem
        {
            Title = request.Title,
            IsCompleted = false,
            DueDate = dueDateUtc,
            Priority = request.Priority,
            UserId = request.UserId
        };

        await _repository.AddAsync(todo);
        
        return todo;
    }
}