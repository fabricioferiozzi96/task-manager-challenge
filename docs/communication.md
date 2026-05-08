# Comunicación: app ↔ API ↔ DB

```mermaid
flowchart LR
    APP["📱 React Native"] -->|HTTP/JSON| API["⚙️ .NET 8 API"]
    API -->|Dapper / SQL| DB[("🗄️ PostgreSQL")]
    DB -->|result sets| API
    API -->|DTOs / errores JSON| APP
```

## Caso típico: filtrar por estado

```mermaid
sequenceDiagram
    actor U as Usuario
    participant App as 📱 App
    participant API as ⚙️ API
    participant Svc as TaskService
    participant DB as 🗄️ Postgres

    U->>App: tap "En progreso"
    App->>App: dispatch setStatus(2)
    App->>API: GET /api/tasks?status=2
    API->>Svc: GetTasksAsync(2, null)
    Svc->>DB: sp_get_tasks(2, NULL)
    DB-->>Svc: 5 rows
    Svc-->>API: List<TaskDto>
    API-->>App: 200 OK + JSON
    App->>App: cache + persistir
    App-->>U: render lista
```

## Caso de error: 404

```mermaid
sequenceDiagram
    participant App as 📱 App
    participant API as ⚙️ API
    participant Svc as TaskService
    participant DB as 🗄️ Postgres

    App->>API: GET /api/tasks/99999
    API->>Svc: GetTaskByIdAsync(99999)
    Svc->>DB: sp_get_task_by_id(99999)
    DB-->>Svc: 0 rows
    Svc->>Svc: throw NotFoundException
    Svc-->>API: ExceptionMiddleware atrapa
    API-->>App: 404 + JSON
    App-->>App: StateMessage error + Reintentar
```

## Contratos en cada hop

| Hop | Forma | Donde vive |
|---|---|---|
| App → API | Query string (`?status=2`) | `tasksApi.ts` |
| API → DB | Función SQL parametrizada | `TaskRepository.cs` |
| DB → API | Result set joineado | `db/03_functions.sql` |
| API → App | JSON con shape `TaskDto` | `Dtos/TaskDto.cs` |
| App interno | `Task` (entity de dominio) | `TaskMapper.toTask` en `transformResponse` |

Cualquier cambio en uno de estos hops solo toca su archivo.

## Persistencia (mobile)

`redux-persist` con AsyncStorage guarda `filters` y el cache de `tasksApi`. Al reabrir la app sin internet, se ve la última lista cargada. Cuando vuelve la conexión, `refetchOnReconnect: true` la actualiza sola.
