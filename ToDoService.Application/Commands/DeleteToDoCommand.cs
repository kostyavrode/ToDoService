using MediatR;

namespace ToDoService.Application.Commands;

public record DeleteToDoCommand(int Id) : IRequest<bool>;