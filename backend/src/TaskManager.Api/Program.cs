using System.Reflection;
using Dapper;
using DotNetEnv;
using FluentValidation;
using MediatR;
using Serilog;
using TaskManager.Api.API.Middleware;
using TaskManager.Api.Application.Behaviors;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Repository;
using TaskManager.Api.Infrastructure.Repository;
using TaskManager.Api.Infrastructure.Service;

// Carga variables desde .env en dev. En prod se setean directamente en el container.
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Logger estructurado.
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console());

// Dapper: mapear columnas snake_case (status_id) → propiedades C# (StatusId).
DefaultTypeMap.MatchNamesWithUnderscores = true;

// ---------------------------------------------------------------
// Composición — el centro de Clean Arch: la API es la única capa
// que conoce a TODOS los demás módulos, porque acá se "atan los cables"
// entre interfaces (Application/Domain) e implementaciones (Infrastructure).
// ---------------------------------------------------------------

// MediatR — descubre todos los IRequest/IRequestHandler del assembly.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Pipeline behavior: corre antes de cada handler para validar el request.
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// FluentValidation — descubre todos los AbstractValidator del assembly.
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Infraestructura: implementaciones concretas detrás de cada interfaz.
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

public partial class Program;
