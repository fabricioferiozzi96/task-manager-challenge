namespace TaskManager.Api.Application.Exceptions;

/// <summary>
/// Excepción que envuelve los errores de FluentValidation cuando alguna regla
/// falla. El ValidationBehavior la lanza desde el pipeline; el middleware en API
/// la traduce a HTTP 400 con el detalle de cada campo inválido.
///
/// Modelo de errores: Dictionary&lt;string, string[]&gt; — clave es el nombre de la
/// propiedad, valor es la lista de mensajes. Mismo shape que usa ASP.NET por
/// defecto cuando rebota un ModelState, así el cliente recibe un formato familiar.
/// </summary>
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}
