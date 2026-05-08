using Dapper;
using DotNetEnv;
using Serilog;
using TaskManager.Api.Middleware;
using TaskManager.Api.Repositories;
using TaskManager.Api.Services;

// Carga variables desde .env si existe (en dev). En producción se setean
// directamente como variables de entorno del proceso/container.
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------
// Logger (Serilog) — equivalente a morgan + winston en Express.
// ---------------------------------------------------------------
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console());

// Dapper: mapear columnas snake_case (status_id) → propiedades C# (StatusId).
DefaultTypeMap.MatchNamesWithUnderscores = true;

// ---------------------------------------------------------------
// Inyección de dependencias — equivalente a "import" + "new" en Express,
// pero el contenedor crea las instancias por nosotros.
// ---------------------------------------------------------------
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p => p
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));

var app = builder.Build();

// ---------------------------------------------------------------
// Pipeline (cada UseX es un app.use(...) de Express, en orden).
// ---------------------------------------------------------------
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();

// Para tests de integración con WebApplicationFactory<Program>.
public partial class Program;
