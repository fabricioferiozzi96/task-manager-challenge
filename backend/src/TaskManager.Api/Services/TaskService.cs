using TaskManager.Api.Dtos;
using TaskManager.Api.Exceptions;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;

namespace TaskManager.Api.Services;

/// <summary>
/// Lógica de negocio. Orquesta el repositorio y mapea entidades a DTOs.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskDto>> GetTasksAsync(int? statusId, int? priorityId, CancellationToken ct)
    {
        var tasks = await _repository.GetAllAsync(statusId, priorityId, ct);
        return tasks.Select(ToDto);
    }

    public async Task<TaskDto> GetTaskByIdAsync(long id, CancellationToken ct)
    {
        var task = await _repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Task with id {id} was not found.");
        return ToDto(task);
    }

    private static TaskDto ToDto(TaskItem t) => new(
        Id:            t.Id,
        Title:         t.Title,
        Description:   t.Description,
        Status:        t.StatusCode,
        StatusLabel:   t.StatusLabel,
        Priority:      t.PriorityCode,
        PriorityLabel: t.PriorityLabel);
}
