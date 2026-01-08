using ToDoService.Domain.Entities;

namespace ToDoService.Application.Interfaces;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<IEnumerable<ToDoItem>> GetByUserIdAsync(int userId);
    Task<ToDoItem?> GetByIdAsync(int id);
    Task AddAsync(ToDoItem item);
    Task UpdateAsync(ToDoItem item);
    Task<bool> DeleteAsync(int id);
}