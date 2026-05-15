using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Application.Query;

namespace TaskManager.Api.API.Controller;
/// 1) Toma los parámetros de la request.
/// 2) Construye el query y lo manda al mediator.
/// 3) Devuelve el resultado con el status code correcto.
///
/// NO valida rangos, NO trae el repositorio, NO mapea DTOs. Eso es lógica
/// del caso de uso y vive en Application.
///
[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Lista tareas con filtros opcionales por estado y prioridad.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TaskListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? status,
        [FromQuery] int? priority,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTasksQuery(status, priority), ct);
        return Ok(result);
    }

    /// <summary>Detalle de una tarea por ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TaskDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id), ct);
        return Ok(result);
    }
}
