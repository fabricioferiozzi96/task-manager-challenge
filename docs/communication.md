# Comunicación: app ↔ API ↔ DB

```mermaid
flowchart LR
    APP["📱 React Native"] -->|HTTP/JSON| API["⚙️ .NET 8 API"]
    API -->|Dapper / StoredProcedure| DB[("🗄️ PostgreSQL")]
    DB -->|result sets| API
    API -->|DTOs / errores JSON| APP
```

## Caso típico: filtrar por estado

```mermaid
sequenceDiagram
    actor U as Usuario
    participant App as 📱 App
    participant API as ⚙️ Controller
    participant MED as MediatR + ValidationBehavior
    participant QH as GetTasksQueryHandler
    participant DB as 🗄️ Postgres

    U->>App: tap "En progreso"
    App->>App: dispatch setStatus(2)
    App->>API: GET /api/tasks?status=2
    API->>MED: mediator.Send(GetTasksQuery(2, null))
    MED->>MED: GetTasksQueryValidator — OK
    MED->>QH: Handle(query)
    QH->>DB: sp_get_tasks(p_status_id=2, p_priority_id=NULL)
    DB-->>QH: 5 rows
    QH->>QH: TaskService.MapToListItem × 5
    QH-->>API: IReadOnlyList<TaskListItemDto>
    API-->>App: 200 OK + JSON
    App->>App: cache + persistir
    App-->>U: render lista
```

## Caso de error: validación fallida (400)

```mermaid
sequenceDiagram
    participant App as 📱 App
    participant API as ⚙️ Controller
    participant MED as MediatR + ValidationBehavior

    App->>API: GET /api/tasks?status=99
    API->>MED: mediator.Send(GetTasksQuery(99, null))
    MED->>MED: GetTasksQueryValidator — FALLA (status fuera de rango 1-4)
    MED-->>API: ValidationException
    API-->>API: ExceptionMiddleware atrapa
    API-->>App: 400 + JSON con detalle por campo
```

## Caso de error: tarea no encontrada (404)

```mermaid
sequenceDiagram
    participant App as 📱 App
    participant API as ⚙️ Controller
    participant MED as MediatR + ValidationBehavior
    participant QH as GetTaskByIdQueryHandler
    participant DB as 🗄️ Postgres

    App->>API: GET /api/tasks/99999
    API->>MED: mediator.Send(GetTaskByIdQuery(99999))
    MED->>MED: GetTaskByIdQueryValidator — OK
    MED->>QH: Handle(query)
    QH->>DB: sp_get_task_by_id(p_id=99999)
    DB-->>QH: 0 rows
    QH->>QH: throw NotFoundException
    QH-->>API: ExceptionMiddleware atrapa
    API-->>App: 404 + JSON
    App-->>App: StateMessage error + Reintentar
```

## Contratos en cada hop

| Hop | Forma | Donde vive |
|---|---|---|
| App → API | Query string (`?status=2`) | `tasksApi.ts` |
| API → MediatR | `GetTasksQuery` / `GetTaskByIdQuery` record | `Application/Query/` |
| MediatR → Handler | request validado por pipeline | `Application/Behaviors/ValidationBehavior.cs` |
| Handler → DB | Stored procedure parametrizado | `Infrastructure/Repository/TaskRepository.cs` |
| DB → Handler | Result set joineado → `TaskRow` → `TaskItem` | `db/03_functions.sql` + `TaskRepository.cs` |
| Handler → API | `IReadOnlyList<TaskListItemDto>` / `TaskDetailDto` | `Application/Dtos/` |
| API → App | JSON con shape del DTO | Swagger / `tasksApi.ts` |
| App interno | `Task` (entity de dominio mobile) | `TaskMapper.toTask` en `transformResponse` |

Cualquier cambio en uno de estos hops solo toca su archivo.

## Persistencia (mobile)

`redux-persist` con AsyncStorage guarda `filters` y el cache de `tasksApi`. Al reabrir la app sin internet, se ve la última lista cargada. Cuando vuelve la conexión, `refetchOnReconnect: true` la actualiza sola.
