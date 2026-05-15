namespace TaskManager.Api.Application.Dtos;
/// DTO liviano para el listado (GET /api/tasks).
///
/// Solo 4 campos: id (para navegar al detalle), title (lo que se muestra),
/// status y priority (los labels para los badges). No incluye:
public record TaskListItemDto(
    long Id,
    string Title,
    string Status,
    string Priority
);
