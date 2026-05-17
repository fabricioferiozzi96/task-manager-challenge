// using MediatR;
// using TaskManager.Api.Application.Commands;
// using TaskManager.Api.Application.Dtos;
// using TaskManager.Api.Application.Services;
// using TaskManager.Api.Domain.Repository;
//
// namespace TaskManager.Api.Application.CommandHandlers;
//
// // Faltaria:
// //   1. Agregar ITaskRepository.CreateAsync en Domain/Repository
// //   2. Implementar TaskRepository.CreateAsync en Infrastructure con un INSERT directo vía Dapper:
// //        INSERT INTO tasks (title, description, status_id, priority_id)
// //        VALUES (@Title, @Description, @StatusId, @PriorityId)
// //        RETURNING id
// //      Luego llamar a GetByIdAsync con el id retornado para hidratar la entidad completa.
// //   3. Agregar CreateTaskCommandValidator en Application/Validators
// public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDetailDto>
// {
//     private readonly ITaskRepository _repository;
//     private readonly ITaskService _service;
//
//     public CreateTaskCommandHandler(ITaskRepository repository, ITaskService service)
//     {
//         _repository = repository;
//         _service = service;
//     }
//
//     public async Task<TaskDetailDto> Handle(CreateTaskCommand request, CancellationToken ct)
//     {
//         var created = await _repository.CreateAsync(
//             title: request.Title,
//             description: request.Description,
//             statusId: request.StatusId,
//             priorityId: request.PriorityId,
//             ct: ct);
//
//         return _service.MapToDetail(created);
//     }
// }
