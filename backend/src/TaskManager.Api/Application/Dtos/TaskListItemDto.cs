namespace TaskManager.Api.Application.Dtos;
public record TaskListItemDto(
    long Id,
    string Title,
    string Status,
    string Priority
);
