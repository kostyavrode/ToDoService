using MediatR;
using ToDoService.Application.Commands;
using ToDoService.Application.Interfaces;
using ToDoService.Domain.Entities;
using BCrypt.Net;

namespace ToDoService.Application.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly IUserRepository _userRepository;

    public RegisterHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByUsernameAsync(request.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);

        return new RegisterResult
        {
            UserId = createdUser.Id,
            Username = createdUser.Username,
            Email = createdUser.Email
        };
    }
}
