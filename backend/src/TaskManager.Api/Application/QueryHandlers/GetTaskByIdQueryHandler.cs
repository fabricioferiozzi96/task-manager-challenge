using MediatR;
using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Application.Query;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Exceptions;
using TaskManager.Api.Domain.Repository;

namespace TaskManager.Api.Application.QueryHandlers;

/// <summary>
/// Caso de uso: "obtener una tarea por id".
///
/// Si no existe, lanza <see cref="NotFoundException"/> — excepción de dominio.
/// La traducción a HTTP 404 la hace el middleware en la capa API; el handler
/// no debe saber nada de status codes.
/// </summary>
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
