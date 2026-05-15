using MediatR;
using TaskManager.Api.Application.Dtos;

namespace TaskManager.Api.Application.Query;
/// incluye campos  (descripción) que se van a mostrar en el detalle.
public record GetTaskByIdQuery(long Id) : IRequest<TaskDetailDto>;
