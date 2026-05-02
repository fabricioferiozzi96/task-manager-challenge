namespace TaskManager.Api.Exceptions;

/// <summary>
/// Se lanza cuando un recurso pedido por id no existe.
/// El middleware la traduce a HTTP 404.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
