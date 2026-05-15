using TaskManager.Api.Domain.Exceptions;
using ValidationException = TaskManager.Api.Application.Exceptions.ValidationException;

namespace TaskManager.Api.API.Middleware;

/// <summary>
/// Único lugar del sistema que traduce excepciones de dominio/app a HTTP.
/// El resto del código lanza excepciones expresivas (NotFoundException,
/// ValidationException) sin saber qué status code va a generar.
///
/// Mapeo:
/// - ValidationException → 400 con el dict de errores por campo.
/// - NotFoundException   → 404 con detalle.
/// - cualquier otra       → 500 con mensaje genérico (el detalle solo al log).
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", ex.Errors);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                status = StatusCodes.Status400BadRequest,
                title = "Validation failed",
                errors = ex.Errors,
                instance = context.Request.Path.Value,
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            await WriteError(context, StatusCodes.Status404NotFound, "Not found", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteError(context, StatusCodes.Status500InternalServerError, "Internal server error",
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static Task WriteError(HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(new
        {
            status,
            title,
            detail,
            instance = context.Request.Path.Value,
        });
    }
}
