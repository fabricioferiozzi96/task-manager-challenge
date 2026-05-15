namespace TaskManager.Api.Application.Dtos;
/// DTO COMPLETO usado por el endpoint de detalle (GET /api/tasks/{id}).
/// Acá sí tiene sentido traer la descripción — es para lo que el usuario
/// abrió la pantalla de detalle.
///
public record TaskDetailDto(
    long Id,
    string Title,
    string? Description,
    string Status,
    string StatusLabel,
    string Priority,
    string PriorityLabel
);
