using Dapper;
using TaskManager.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Mapear columnas snake_case (status_id) a propiedades C# PascalCase (StatusId).
DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddSingleton<ITaskRepository, TaskRepository>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();

public partial class Program;
