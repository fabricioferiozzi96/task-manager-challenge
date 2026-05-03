using TaskManager.Api.Models;

namespace TaskManager.UnitTests.TestFixtures;

/// <summary>
/// Construye TaskItems plausibles para los tests sin tener que repetir todos los campos.
/// </summary>
internal static class TaskItemFixture
{
    public static TaskItem Create(
        long id = 1,
        string title = "Sample task",
        string? description = "Some description",
        short statusId = 1,
        string statusCode = "pending",
        string statusLabel = "Pendiente",
        short priorityId = 2,
        string priorityCode = "medium",
        string priorityLabel = "Media") => new()
        {
            Id = id,
            Title = title,
            Description = description,
            StatusId = statusId,
            StatusCode = statusCode,
            StatusLabel = statusLabel,
            PriorityId = priorityId,
            PriorityCode = priorityCode,
            PriorityLabel = priorityLabel,
        };
}
