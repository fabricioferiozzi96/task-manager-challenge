namespace TaskManager.Api.Application.Dtos;
public record TaskDetailDto(
    long Id,
    string Title,
    string? Description,
    string Status,
    string StatusLabel,
    string Priority,
    string PriorityLabel
);
