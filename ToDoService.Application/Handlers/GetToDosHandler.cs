using MediatR;
using ToDoService.Application.Interfaces;
using ToDoService.Application.Queries;
using ToDoService.Domain.Entities;

namespace ToDoService.Application.Handlers;

public class GetToDosHandler : IRequestHandler<GetToDoQuery, IEnumerable<ToDoItem>>
{
    private readonly IToDoRepository _repository;

    public GetToDosHandler(IToDoRepository repository)
    {
        _repository = repository;
    }    
    
    public async Task<IEnumerable<ToDoItem>> Handle(GetToDoQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}