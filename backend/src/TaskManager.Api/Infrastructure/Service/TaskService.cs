using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Infrastructure.Service;

/// <summary>
/// Implementación del <see cref="ITaskService"/>. Hoy hace mapeo entidad → DTO,
/// pero el lugar está pensado para crecer:
/// - Notificar (mail/push) cuando una tarea cambia.
/// - Llamar a un servicio externo de etiquetado/clasificación.
/// - Aplicar políticas que combinen varias fuentes (cache, DB, API externa).
///
/// Por eso vive en Infrastructure: en el momento que entre cualquiera de esas
/// integraciones, este service ya conoce las dependencias concretas (SmtpClient,
/// HttpClient, IDistributedCache). Si terminara siendo solo mapeo puro forever,
/// se podría plegar dentro de los handlers — pero la interfaz queda como punto
/// de extensión barato.
/// </summary>
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
