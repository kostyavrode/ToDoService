using MediatR;
using ToDoService.Domain.Entities;
using ToDoService.Domain.Enums;

namespace ToDoService.Application.Commands;

public record UpdateToDoCommand(int Id, string? Title = null, bool? IsCompleted = null, DateTime? DueDate = null, Priority? Priority = null) : IRequest<ToDoItem>;