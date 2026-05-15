using TaskManager.Api.Domain.Entities;
using DomainTaskStatus = TaskManager.Api.Domain.Entities.TaskStatus;
using DomainTaskPriority = TaskManager.Api.Domain.Entities.TaskPriority;

namespace TaskManager.UnitTests.TestFixtures;

/// <summary>
/// Construye TaskItems plausibles para los tests sin tener que repetir todos los campos.
/// Ahora arma value objects (TaskStatus, TaskPriority) porque la entidad ya no expone
/// id/code/label sueltos.
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
        string priorityLabel = "Media") => new(
            id: id,
            title: title,
            description: description,
            status: new DomainTaskStatus(statusId, statusCode, statusLabel),
            priority: new DomainTaskPriority(priorityId, priorityCode, priorityLabel));
}
