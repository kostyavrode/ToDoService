using ToDoService.Domain.Enums;

namespace ToDoServoce.Api.Dtos;

public class UpdateToDoDto
{
    public string? Title { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority? Priority { get; set; }
}
