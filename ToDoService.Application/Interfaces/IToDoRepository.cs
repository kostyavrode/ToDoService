using ToDoService.Domain.Entities;

namespace ToDoService.Application.Interfaces;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();

    Task AddAsync(ToDoItem item);
    
    Task<bool> DeleteAsync(int id);
}