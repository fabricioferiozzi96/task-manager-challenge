namespace TaskManager.Api.Application.Dtos;

/// <summary>
/// DTO COMPLETO usado por el endpoint de detalle (GET /api/tasks/{id}).
/// Acá sí tiene sentido traer la descripción — es para lo que el usuario
/// abrió la pantalla de detalle.
///
/// Tener DTOs separados (no uno solo "TaskDto" con campos opcionales) deja
/// el contrato explícito en Swagger/OpenAPI y permite evolucionarlos por
/// separado (ej. detalle suma "createdAt", listado suma "thumbnail").
/// </summary>
public record TaskDetailDto(
    long Id,
    string Title,
    string? Description,
    string Status,
    string StatusLabel,
    string Priority,
    string PriorityLabel
);
