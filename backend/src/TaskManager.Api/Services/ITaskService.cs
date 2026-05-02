using TaskManager.Api.Dtos;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetTasksAsync(int? statusId, int? priorityId, CancellationToken ct);

    /// <summary>Devuelve la tarea o lanza <see cref="Exceptions.NotFoundException"/>.</summary>
    Task<TaskDto> GetTaskByIdAsync(long id, CancellationToken ct);
}
