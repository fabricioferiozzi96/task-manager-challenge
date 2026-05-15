using MediatR;
using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Application.Query;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Repository;

namespace TaskManager.Api.Application.QueryHandlers;

/// <summary>
/// Caso de uso: "listar tareas con filtros opcionales".
///
/// Es el corazón de Clean Arch + CQRS:
/// - Recibe la query (input ya validado por FluentValidation vía pipeline behavior).
/// - Pide al repositorio los datos (depende de la abstracción, no de Dapper/Postgres).
/// - Usa el TaskService para mapear cada entidad al DTO liviano.
///
/// </summary>
public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, IReadOnlyList<TaskListItemDto>>
{
    private readonly ITaskRepository _repository;
    private readonly ITaskService _service;

    public GetTasksQueryHandler(ITaskRepository repository, ITaskService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<IReadOnlyList<TaskListItemDto>> Handle(GetTasksQuery request, CancellationToken ct)
    {
        var tasks = await _repository.GetAllAsync(request.StatusId, request.PriorityId, ct);
        return tasks.Select(_service.MapToListItem).ToList();
    }
}
