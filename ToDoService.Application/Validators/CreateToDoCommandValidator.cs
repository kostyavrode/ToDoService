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
        
        When(x => x.DueDate.HasValue, () =>
        {
            RuleFor(x => x.DueDate!.Value)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Due date cannot be in the past");
        });
    }
}