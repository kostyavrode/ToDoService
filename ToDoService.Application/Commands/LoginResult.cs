namespace ToDoService.Application.Commands;

public class LoginResult
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
