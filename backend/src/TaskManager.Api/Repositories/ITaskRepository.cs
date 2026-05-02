using TaskManager.Api.Models;

namespace TaskManager.Api.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync(int? statusId, int? priorityId, CancellationToken ct);
    Task<TaskItem?> GetByIdAsync(long id, CancellationToken ct);
}
