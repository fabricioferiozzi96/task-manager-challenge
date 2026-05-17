using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Domain.Repository;
public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync(int? statusId, int? priorityId, CancellationToken ct);
    Task<TaskItem?> GetByIdAsync(long id, CancellationToken ct);
}
