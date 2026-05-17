using System.Data;
using Dapper;
using Npgsql;
using TaskManager.Api.Domain.Entities;
using TaskManager.Api.Domain.Repository;
using TaskManager.Api.Infrastructure.Persistence;
using DomainTaskStatus = TaskManager.Api.Domain.Entities.TaskStatus;
using DomainTaskPriority = TaskManager.Api.Domain.Entities.TaskPriority;

namespace TaskManager.Api.Infrastructure.Repository;

/// <summary>
/// Implementación del repositorio con Dapper + Npgsql invocando los stored procedures
/// sp_get_tasks y sp_get_task_by_id via CommandType.StoredProcedure.
/// Vive en Infrastructure porque conoce detalles concretos: cadena de conexión, dialecto SQL, Dapper.
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
        await using var conn = new NpgsqlConnection(_connectionString);
        var rows = await conn.QueryAsync<TaskRow>(new CommandDefinition(
            "sp_get_tasks",
            new { p_status_id = (short?)statusId, p_priority_id = (short?)priorityId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));

        return rows.Select(ToDomain).ToList();
    }

    public async Task<TaskItem?> GetByIdAsync(long id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        var row = await conn.QuerySingleOrDefaultAsync<TaskRow>(new CommandDefinition(
            "sp_get_task_by_id",
            new { p_id = id },
            commandType: CommandType.StoredProcedure,
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
