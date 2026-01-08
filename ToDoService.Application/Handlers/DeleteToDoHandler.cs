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
        return await _repository.DeleteAsync(request.Id);
    }
}