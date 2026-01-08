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
        var todo = new ToDoItem
        {
            Title = request.Title,
            IsCompleted = false
        };

        await _repository.AddAsync(todo);
        
        return todo;
    }
}