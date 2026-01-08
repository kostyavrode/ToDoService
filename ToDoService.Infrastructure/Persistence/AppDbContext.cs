using Microsoft.EntityFrameworkCore;
using ToDoService.Domain.Entities;

namespace ToDoService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ToDoItem> ToDos => Set<ToDoItem>();
}