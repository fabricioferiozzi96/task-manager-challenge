using MediatR;
using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Application.Query;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Exceptions;
using TaskManager.Api.Domain.Repository;

namespace TaskManager.Api.Application.QueryHandlers;
public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailDto>
{
    private readonly ITaskRepository _repository;
    private readonly ITaskService _service;

    public GetTaskByIdQueryHandler(ITaskRepository repository, ITaskService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<TaskDetailDto> Handle(GetTaskByIdQuery request, CancellationToken ct)
    {
        var task = await _repository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException($"Task with id {request.Id} was not found.");

        return _service.MapToDetail(task);
    }
}
