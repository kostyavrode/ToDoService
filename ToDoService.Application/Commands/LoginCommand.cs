using MediatR;

namespace ToDoService.Application.Commands;

public record LoginCommand(string Username, string Password) : IRequest<LoginResult?>;
