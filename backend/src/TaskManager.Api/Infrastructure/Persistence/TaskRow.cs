namespace TaskManager.Api.Infrastructure.Persistence;

/// <summary>
/// "Fila de DB" — POCO plano con setters públicos que Dapper hidrata desde
/// el resultado del SELECT. Vive en Infrastructure, NO en Domain.
///
/// ¿Por qué separar esto de la entidad TaskItem?
/// - Dapper necesita setters públicos para mapear; la entidad de dominio
///   tiene setters privados y constructor con invariantes. Si la entidad
///   tuviera que servir las dos cosas, perderíamos la integridad del modelo.
/// - El shape de la DB (snake_case, ids como short, etc.) es un detalle de
///   infraestructura que NO debería contaminar el dominio.
/// - Si mañana cambia la DB (otra tabla, otro engine), solo cambia esta capa
///   y el mapeo; el dominio queda intacto.
///
/// Description es nullable porque el SELECT del listado no la trae —
/// proyectamos columnas distintas según el caso de uso (ver TaskRepository).
/// </summary>
public class TaskRow
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public short StatusId { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;

    public short PriorityId { get; set; }
    public string PriorityCode { get; set; } = string.Empty;
    public string PriorityLabel { get; set; } = string.Empty;
}
