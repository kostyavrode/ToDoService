using ToDoService.Domain.Enums;

namespace ToDoService.Domain.Entities;

public class ToDoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}