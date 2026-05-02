namespace TaskManager.Api.Dtos;

/// <summary>
/// Forma exacta del JSON que devuelve la API al cliente.
/// Renombramos StatusCode → Status / PriorityCode → Priority para una shape más limpia.
/// </summary>
public record TaskDto(
    long Id,
    string Title,
    string? Description,
    string Status,
    string StatusLabel,
    string Priority,
    string PriorityLabel
);
