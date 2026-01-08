using Microsoft.EntityFrameworkCore;
using ToDoService.Application.Interfaces;
using ToDoService.Domain.Entities;
using ToDoService.Infrastructure.Persistence;

namespace ToDoService.Infrastructure.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;

    public ToDoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        return await _context.ToDos.ToListAsync();
    }

    public async Task AddAsync(ToDoItem item)
    {
        _context.ToDos.Add(item);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var todo = await _context.ToDos.FindAsync(id);

        if (todo == null)
            return false;

        _context.ToDos.Remove(todo);
        await _context.SaveChangesAsync();
        return true;
    }

}