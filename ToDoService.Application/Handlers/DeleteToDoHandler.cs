using MediatR;
using ToDoService.Application.Commands;
using ToDoService.Application.Interfaces;

namespace ToDoService.Application.Handlers;

public class DeleteToDoHandler : IRequestHandler<DeleteToDoCommand, bool>
{
    private readonly IToDoRepository _repository;

    public DeleteToDoHandler(IToDoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetByIdAsync(request.Id);
        
        if (todo == null)
        {
            return false;
        }

        if (todo.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You don't have permission to delete this item");
        }

        return await _repository.DeleteAsync(request.Id);
    }
}