namespace TaskManager.Api.Domain.Entities;

/// <summary>
/// Value Object para la prioridad. Mismo razonamiento que <see cref="TaskStatus"/>:
/// los valores los provee la tabla task_priority, no un enum hardcodeado.
/// </summary>
public sealed class TaskPriority
{
    public short Id { get; }
    public string Code { get; }
    public string Label { get; }

    public TaskPriority(short id, string code, string label)
    {
        if (id <= 0) throw new ArgumentException("Priority id must be positive.", nameof(id));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Priority code required.", nameof(code));
        if (string.IsNullOrWhiteSpace(label)) throw new ArgumentException("Priority label required.", nameof(label));

        Id = id;
        Code = code;
        Label = label;
    }
}
