using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Infrastructure.Service;
public class TaskService : ITaskService
{
    public TaskListItemDto MapToListItem(TaskItem t) => new(
        Id:       t.Id,
        Title:    t.Title,
        Status:   t.Status.Label,
        Priority: t.Priority.Label);

    public TaskDetailDto MapToDetail(TaskItem t) => new(
        Id:            t.Id,
        Title:         t.Title,
        Description:   t.Description,
        Status:        t.Status.Code,
        StatusLabel:   t.Status.Label,
        Priority:      t.Priority.Code,
        PriorityLabel: t.Priority.Label);
}
