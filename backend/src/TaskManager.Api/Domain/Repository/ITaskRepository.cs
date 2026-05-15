using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Domain.Repository;

/// <summary>
/// Contrato del repositorio. Vive en Domain porque el dominio define QUÉ necesita
/// para funcionar (cómo persistir/leer tareas), sin saber CÓMO se hace
/// (Dapper, EF, Mongo, archivo plano — eso es decisión de Infrastructure).
///
/// Inversión de dependencias: Application/Domain dependen de esta abstracción;
/// Infrastructure provee la implementación. Esto permite swappear Postgres
/// por otra cosa sin tocar handlers ni dominio, y testear con mocks/fakes.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Listado liviano: NO trae la descripción (puede ser pesada).
    /// Esto se refleja también en el SQL — el repo proyecta solo las columnas
    /// que necesita el caso de uso "listar".
    /// </summary>
    Task<IReadOnlyList<TaskItem>> GetAllAsync(int? statusId, int? priorityId, CancellationToken ct);

    /// <summary>
    /// Detalle completo: trae la descripción y cualquier otro campo "pesado".
    /// </summary>
    Task<TaskItem?> GetByIdAsync(long id, CancellationToken ct);
}
