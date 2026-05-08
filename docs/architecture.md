# Arquitectura del backend

Layered clásica en un solo proyecto. Tres capas, una sola dirección de dependencia.

```mermaid
flowchart TD
    HTTP["HTTP Request"] --> CTRL["TasksController"]
    CTRL --> SVC["TaskService"]
    SVC --> REPO["TaskRepository"]
    REPO -->|Dapper| DB[("PostgreSQL")]

    SVC -.->|throw NotFoundException| MID["ExceptionMiddleware"]
    MID -.->|JSON 404/500| HTTP

    style CTRL fill:#fce7f3,stroke:#db2777
    style SVC fill:#dbeafe,stroke:#2563eb
    style REPO fill:#dcfce7,stroke:#16a34a
    style MID fill:#fef3c7,stroke:#d97706
```

## Responsabilidad por capa

- **Controller**: recibe la request, valida query params básicos y delega.
- **Service**: orquesta el repo y mapea `TaskItem` → `TaskDto`. Lanza `NotFoundException` cuando un id no existe.
- **Repository**: único lugar que conoce SQL. Usa Dapper para invocar `sp_get_tasks` y `sp_get_task_by_id`.
- **Middleware**: traduce excepciones a JSON consistente (status, title, detail, instance).

## Composición (`Program.cs`)

```csharp
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
```

Repo y service Scoped (instancia por request). Es la convención en .NET y evita problemas si más adelante el repo recibe una dependencia con scope.

## Configuración

`DotNetEnv.Env.TraversePath().Load()` al arranque carga `.env` en variables de entorno. .NET las lee como `IConfiguration` con la convención `Section__Key` (ej. `ConnectionStrings__Default`). En producción se setean directamente en el container, sin archivo.
