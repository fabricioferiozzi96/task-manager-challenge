namespace TaskManager.Api.Models;

/// <summary>
/// Modelo plano que representa una tarea tal como la devuelven las funciones de la DB
/// (sp_get_tasks, sp_get_task_by_id). Dapper mapea las columnas snake_case (status_id)
/// a estas propiedades PascalCase (StatusId) gracias a MatchNamesWithUnderscores en Program.cs.
/// </summary>
public class TaskItem
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public short StatusId { get; set; }
    public string StatusCode { get; set; } = string.Empty;   // "in_progress"
    public string StatusLabel { get; set; } = string.Empty;  // "En progreso"

    public short PriorityId { get; set; }
    public string PriorityCode { get; set; } = string.Empty;  // "urgent"
    public string PriorityLabel { get; set; } = string.Empty; // "Urgente"


}
