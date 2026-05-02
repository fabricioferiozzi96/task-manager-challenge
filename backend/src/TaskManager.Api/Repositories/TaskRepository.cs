using Dapper;
using Npgsql;
using TaskManager.Api.Models;

namespace TaskManager.Api.Repositories;

/// <summary>
/// Acceso a datos vía Dapper + Npgsql. Llama a las funciones SQL (sp_get_tasks y
/// sp_get_task_by_id) — equivalente a un repositorio que invoca stored procedures.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string 'Default' in appsettings.");
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(int? statusId, int? priorityId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<TaskItem>(new CommandDefinition(
            "SELECT * FROM sp_get_tasks(@p_status_id, @p_priority_id)",
            new { p_status_id = (short?)statusId, p_priority_id = (short?)priorityId },
            cancellationToken: ct));
    }

    public async Task<TaskItem?> GetByIdAsync(long id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<TaskItem>(new CommandDefinition(
            "SELECT * FROM sp_get_task_by_id(@p_id)",
            new { p_id = id },
            cancellationToken: ct));
    }
}
