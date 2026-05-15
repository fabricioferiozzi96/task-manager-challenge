using Dapper;
using Npgsql;
using TaskManager.Api.Domain.Entities;
using TaskManager.Api.Domain.Repository;
using TaskManager.Api.Infrastructure.Persistence;
using DomainTaskStatus = TaskManager.Api.Domain.Entities.TaskStatus;
using DomainTaskPriority = TaskManager.Api.Domain.Entities.TaskPriority;

namespace TaskManager.Api.Infrastructure.Repository;

/// <summary>
/// Implementación del repositorio con Dapper + Npgsql sobre las funciones SQL
/// (sp_get_tasks, sp_get_task_by_id). Vive en Infrastructure porque conoce
/// detalles concretos: la cadena de conexión, el dialecto SQL, Dapper.
///
/// Dos métodos, dos SELECTs distintos — esto es lo que el feedback de la
/// entrevista pedía:
/// - GetAllAsync proyecta SOLO las columnas que necesita el listado (sin description).
///   Ahorra bytes desde la DB hasta la respuesta HTTP.
/// - GetByIdAsync hace SELECT * porque el detalle sí necesita todo.
///
/// Mapeo: las filas (TaskRow) se traducen a entidades de dominio (TaskItem)
/// antes de salir del repositorio. El resto del sistema solo ve TaskItem.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string 'Default' in appsettings.");
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(int? statusId, int? priorityId, CancellationToken ct)
    {
        const string sql = @"
            SELECT id, title,
                   status_id, status_code, status_label,
                   priority_id, priority_code, priority_label
            FROM sp_get_tasks(@p_status_id, @p_priority_id)";

        await using var conn = new NpgsqlConnection(_connectionString);
        var rows = await conn.QueryAsync<TaskRow>(new CommandDefinition(
            sql,
            new { p_status_id = (short?)statusId, p_priority_id = (short?)priorityId },
            cancellationToken: ct));

        return rows.Select(ToDomain).ToList();
    }

    public async Task<TaskItem?> GetByIdAsync(long id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        var row = await conn.QuerySingleOrDefaultAsync<TaskRow>(new CommandDefinition(
            "SELECT * FROM sp_get_task_by_id(@p_id)",
            new { p_id = id },
            cancellationToken: ct));

        return row is null ? null : ToDomain(row);
    }

    private static TaskItem ToDomain(TaskRow r) => new(
        id: r.Id,
        title: r.Title,
        description: r.Description,
        status: new DomainTaskStatus(r.StatusId, r.StatusCode, r.StatusLabel),
        priority: new DomainTaskPriority(r.PriorityId, r.PriorityCode, r.PriorityLabel));
}
