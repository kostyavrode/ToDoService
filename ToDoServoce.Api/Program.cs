using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoService.Application.Commands;
using ToDoService.Application.Interfaces;
using ToDoService.Application.Queries;
using ToDoService.Infrastructure.Persistence;
using ToDoService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(
    typeof(CreateTodoCommandValidator).Assembly);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>)
);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateToDoCommand).Assembly);
});
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=todo_db;Username=postgres;Password=postgres"));

builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

