using FluentValidation;
using ToDoService.Application.Commands;

namespace ToDoService.Application.Validators;

public class UpdateToDoCommandValidators : AbstractValidator<UpdateToDoCommand>
{
    public UpdateToDoCommandValidators()
    {
        RuleFor (x => x.Id).GreaterThan (0).WithMessage ("Id must be greater than 0");

        When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title!).NotEmpty().MaximumLength(100).MinimumLength(3);
            }
        );

        When(x => x.DueDate.HasValue, () =>
        {
            RuleFor(x => x.DueDate!).GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Due date must be greater than today");
        });
    }
}