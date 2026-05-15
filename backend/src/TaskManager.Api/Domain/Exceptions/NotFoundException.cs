namespace TaskManager.Api.Domain.Exceptions;

/// <summary>
/// Excepción de dominio: el recurso pedido por id no existe.
///
/// Vive en Domain (no en API) porque expresar "no existe" es un concepto del
/// negocio, no de HTTP. El middleware en la capa API es quien traduce esto
/// a un 404 — el dominio no sabe nada de HTTP.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
