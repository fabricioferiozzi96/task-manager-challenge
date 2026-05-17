# Backend - Task Manager API

API REST en .NET 8 con Clean Architecture + CQRS (lado Query), MediatR, FluentValidation y Dapper sobre PostgreSQL.

## Estructura

```
src/TaskManager.Api/
├── API/            Controller (anémico: solo mediator.Send) + ExceptionMiddleware
├── Application/    Queries, QueryHandlers, Validators, ValidationBehavior, DTOs, ITaskService
├── Domain/         TaskItem (rich model), VOs (TaskStatus, TaskPriority), ITaskRepository, NotFoundException
└── Infrastructure/ TaskRepository (Dapper), TaskService (mapeo), TaskRow (POCO Dapper)
```

Las dependencias van siempre hacia adentro. Domain no depende de nada. API es el composition root.

## Por qué estas decisiones

**Clean Architecture + CQRS**: cada capa tiene una sola razón para cambiar. CQRS solo en el lado Query porque solo hay 2 Gets; la estructura ya está preparada con `Command/CommandHandlers` para agregar operaciones que modifican estado.

**FluentValidation**: porque las reglas viven en `Application/Validators`, no inline en el controller. Si la validación falla, `ValidationException` llega al middleware y el handler nunca se ejecuta.

**Dos DTOs**: `TaskListItemDto` para el listado con menos campos porque asi mejora la performance en caso de que el listado de tareas tenga muchas filas; `TaskDetailDto` (7 campos) para el detalle.

**Dapper sobre EF Core**: el reto pide stored procedures. Dapper ejecuta el SP, mapea el result set y listo. EF Core trae change-tracking y migrations que no se usan en un flujo de solo lectura vía SP.

## Stack

- MediatR 12 · FluentValidation 11 · Dapper 2 + Npgsql 8 · Serilog 8 · Swashbuckle
- xUnit + NSubstitute + FluentAssertions

## Setup

```powershell
cd src/TaskManager.Api
Copy-Item .env.example .env   # ajustar credenciales si hace falta

dotnet restore
dotnet build
dotnet test

dotnet run
# http://localhost:5186
# Swagger: http://localhost:5186/swagger
```

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/tasks` | Lista tareas. Filtros: `?status=&priority=` |
| GET | `/api/tasks/{id}` | Detalle. 404 si no existe. |
| GET | `/health` | Healthcheck. |

Status: `1=pending` `2=in_progress` `3=completed` `4=cancelled`  
Priority: `1=low` `2=medium` `3=high` `4=urgent`

## Flujo de un request

`GET /api/tasks?status=2`

1. **Controller** — construye `GetTasksQuery(2, null)` → `mediator.Send`
2. **ValidationBehavior** — corre `GetTasksQueryValidator`. Si falla → `ValidationException` → 400
3. **GetTasksQueryHandler** — llama a `ITaskRepository` y mapea con `ITaskService`
4. **TaskRepository** — ejecuta `sp_get_tasks` vía Dapper, mapea `TaskRow → TaskItem`
5. Handler retorna `IReadOnlyList<TaskListItemDto>` → controller responde 200
