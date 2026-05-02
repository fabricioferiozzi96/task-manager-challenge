using Dapper;
using TaskManager.Api.Repositories;
using TaskManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Mapear columnas snake_case (status_id) a propiedades C# PascalCase (StatusId).
DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();

public partial class Program;
