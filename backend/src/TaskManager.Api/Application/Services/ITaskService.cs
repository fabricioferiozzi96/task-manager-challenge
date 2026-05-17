using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.Services;
public interface ITaskService
{
    TaskListItemDto MapToListItem(TaskItem task);
    TaskDetailDto MapToDetail(TaskItem task);
}
