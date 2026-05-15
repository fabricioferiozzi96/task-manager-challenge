using MediatR;
using TaskManager.Api.Application.Dtos;

namespace TaskManager.Api.Application.Query;

/// <summary>
/// Query del detalle. Devuelve <see cref="TaskDetailDto"/> — distinto al de la lista,
/// porque incluye campos pesados (descripción) que solo importan en detalle.
/// </summary>
public record GetTaskByIdQuery(long Id) : IRequest<TaskDetailDto>;
