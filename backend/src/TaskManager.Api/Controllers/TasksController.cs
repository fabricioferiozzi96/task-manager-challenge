using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Dtos;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    /// <summary>Lista tareas con filtros opcionales por estado y prioridad.</summary>
    /// <param name="status">ID de estado (1=pendiente, 2=en progreso, 3=completada, 4=cancelada).</param>
    /// <param name="priority">ID de prioridad (1=baja, 2=media, 3=alta, 4=urgente).</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? status,
        [FromQuery] int? priority,
        CancellationToken ct)
    {
        if (status is < 1 or > 4)
            return BadRequest(new { error = "Invalid status. Must be between 1 and 4." });
        if (priority is < 1 or > 4)
            return BadRequest(new { error = "Invalid priority. Must be between 1 and 4." });

        var tasks = await _service.GetTasksAsync(status, priority, ct);
        return Ok(tasks);
    }

    /// <summary>Detalle de una tarea por ID.</summary>
    /// <param name="id">ID de la tarea.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var task = await _service.GetTaskByIdAsync(id, ct);
        return Ok(task);
    }
}
