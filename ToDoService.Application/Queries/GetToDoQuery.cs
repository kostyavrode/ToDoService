using MediatR;
using ToDoService.Domain.Entities;

namespace ToDoService.Application.Queries;

public record GetToDoQuery(int UserId) : IRequest<IEnumerable<ToDoItem>>;