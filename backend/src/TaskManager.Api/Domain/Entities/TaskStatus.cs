namespace TaskManager.Api.Domain.Entities;

/// <summary>
/// Value Object. Encapsula los tres datos que conforman un estado (id, code, label).
///
/// ¿Por qué un VO y no un enum?
/// - Los códigos y labels viven en la tabla task_status (1=pending, 2=in_progress, …).
///   Un enum hardcodea esos valores en código; agregar uno nuevo requiere recompilar.
/// - Con VO, la DB es la fuente de verdad y el dominio sigue siendo tipado.
/// - Soporta metadata futura (color, orden) sin tocar a TaskItem.
/// </summary>
public sealed class TaskStatus
{
    public short Id { get; }
    public string Code { get; }
    public string Label { get; }

    public TaskStatus(short id, string code, string label)
    {
        if (id <= 0) throw new ArgumentException("Status id must be positive.", nameof(id));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Status code required.", nameof(code));
        if (string.IsNullOrWhiteSpace(label)) throw new ArgumentException("Status label required.", nameof(label));

        Id = id;
        Code = code;
        Label = label;
    }
}
