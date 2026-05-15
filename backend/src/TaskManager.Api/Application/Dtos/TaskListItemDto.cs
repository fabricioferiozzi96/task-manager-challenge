namespace TaskManager.Api.Application.Dtos;

/// <summary>
/// DTO liviano para el listado (GET /api/tasks).
///
/// Solo 4 campos: id (para navegar al detalle), title (lo que se muestra),
/// status y priority (los labels para los badges). No incluye:
/// - Description: campo pesado, vive en TaskDetailDto.
/// - Codes (status_code, priority_code): redundantes con el label en una lista
///   pura de display. Si el frontend necesita filtrar por código, ya conoce
///   el valor — lo envió como filtro.
/// </summary>
public record TaskListItemDto(
    long Id,
    string Title,
    string Status,
    string Priority
);
