using MediatR;
using TaskManager.Api.Application.Dtos;

namespace TaskManager.Api.Application.Query;
public record GetTasksQuery(int? StatusId, int? PriorityId) : IRequest<IReadOnlyList<TaskListItemDto>>;
