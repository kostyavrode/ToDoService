using FluentValidation;
using ToDoService.Application.Commands;

public class CreateTodoCommandValidator 
    : AbstractValidator<CreateToDoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}