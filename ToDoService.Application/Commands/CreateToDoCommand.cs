using ToDoService.Domain.Entities;
using MediatR;

namespace ToDoService.Application.Commands;

public record CreateToDoCommand(string Title) : IRequest<ToDoItem>;