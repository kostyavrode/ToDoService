using MediatR;
using ToDoService.Application.Interfaces;
using ToDoService.Application.Queries;
using ToDoService.Domain.Entities;

namespace ToDoService.Application.Handlers;

public class GetToDoByIdHandler : IRequestHandler<GetToDoByIdQuery, ToDoItem?>
{
    private readonly IToDoRepository _repository;

    public GetToDoByIdHandler(IToDoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ToDoItem?> Handle(GetToDoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}