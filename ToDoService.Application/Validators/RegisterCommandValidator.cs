using FluentValidation;
using ToDoService.Application.Commands;
using ToDoService.Application.Interfaces;

namespace ToDoService.Application.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .MustAsync(async (username, cancellation) => 
                !await _userRepository.ExistsByUsernameAsync(username))
            .WithMessage("Username already exists");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100)
            .MustAsync(async (email, cancellation) => 
                !await _userRepository.ExistsByEmailAsync(email))
            .WithMessage("Email already exists");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);
    }
}
