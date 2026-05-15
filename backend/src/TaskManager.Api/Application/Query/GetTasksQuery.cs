using MediatR;
using TaskManager.Api.Application.Dtos;

namespace TaskManager.Api.Application.Query;
/// CQRS — lado "Query": pregunta inmutable que NO modifica estado.
/// MediatR la lleva al QueryHandler correspondiente.
/// ¿Por qué?
/// - El handler recibe UN objeto: facilita validación (FluentValidation
///   tiene un validator por query), logging y eventual middleware (caching,
///   métricas) vía pipeline behaviors.
/// - El controller solo sabe construir esta cosa, no conoce repositorios ni
///   reglas de filtrado. Separa transporte de caso de uso.
public record GetTasksQuery(int? StatusId, int? PriorityId) : IRequest<IReadOnlyList<TaskListItemDto>>;
