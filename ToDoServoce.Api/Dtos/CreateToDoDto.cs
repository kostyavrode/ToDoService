using ToDoService.Domain.Enums;

namespace ToDoServoce.Api.Dtos;

public class CreateToDoDto
{
    public string Title { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
}