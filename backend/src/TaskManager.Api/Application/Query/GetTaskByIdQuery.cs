using MediatR;
using TaskManager.Api.Application.Dtos;

namespace TaskManager.Api.Application.Query;
public record GetTaskByIdQuery(long Id) : IRequest<TaskDetailDto>;
