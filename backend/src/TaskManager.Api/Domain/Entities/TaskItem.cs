namespace TaskManager.Api.Domain.Entities;

/// <summary>
/// Entidad de dominio. Representa una Tarea como concepto del negocio,
/// no como fila de la DB ni como JSON expuesto al cliente.
///
/// Diseño:
/// - Constructor con todos los invariantes obligatorios.
/// - Setters privados: nadie de afuera puede mutar el estado a mano.
/// - Validaciones de invariantes acá adentro (título no vacío, status/priority válidos).
///   El controller no necesita saber qué es un título válido — eso vive en el dominio.
/// </summary>
public sealed class TaskItem
{
    public long Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }

    public TaskItem(
        long id,
        string title,
        string? description,
        TaskStatus status,
        TaskPriority priority)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be positive.", nameof(id));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        Id = id;
        Title = title;
        Description = description;
        Status = status ?? throw new ArgumentNullException(nameof(status));
        Priority = priority ?? throw new ArgumentNullException(nameof(priority));
    }
}
