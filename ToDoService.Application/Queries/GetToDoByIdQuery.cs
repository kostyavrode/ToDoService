using MediatR;
using ToDoService.Domain.Entities;

namespace ToDoService.Application.Queries;

public record GetToDoByIdQuery(int Id) : IRequest<ToDoItem?>;