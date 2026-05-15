using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.Services;

/// <summary>
/// Contrato de un servicio de tareas — operaciones que NO son acceso a datos puro.
///
/// En este proyecto cumple dos roles:
/// 1) Mapeo entidad de dominio → DTO de salida (separa "qué traigo de la DB" de
///    "qué le devuelvo al cliente"). El handler no se ensucia con shape de JSON.
/// 2) Lugar natural para sumar lógica más adelante: integraciones externas
///    (notificar, enviar email cuando una tarea cambia), políticas que crucen
///    varias entidades, cálculos derivados.
///
/// La interfaz vive en Application (define QUÉ se necesita); la implementación
/// vive en Infrastructure (define CÓMO) — misma inversión de dependencias que
/// con el repositorio.
/// </summary>
public interface ITaskService
{
    TaskListItemDto MapToListItem(TaskItem task);
    TaskDetailDto MapToDetail(TaskItem task);
}
