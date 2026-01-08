using ToDoService.Domain.Entities;
using MediatR;
using ToDoService.Domain.Enums;

namespace ToDoService.Application.Commands;

public record CreateToDoCommand(string Title, DateTime? DueDate = null, Priority Priority = Priority.Medium) : IRequest<ToDoItem>;