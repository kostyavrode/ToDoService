using MediatR;

namespace ToDoService.Application.Commands;

public record DeleteToDoCommand(int Id, int UserId) : IRequest<bool>;