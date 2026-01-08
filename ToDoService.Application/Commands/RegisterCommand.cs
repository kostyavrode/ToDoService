using MediatR;

namespace ToDoService.Application.Commands;

public record RegisterCommand(string Username, string Email, string Password) : IRequest<RegisterResult>;
