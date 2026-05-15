# Backend - Task Manager API

API REST en .NET 8 con **Clean Architecture + CQRS** (lado Query). Capas separadas (Domain / Application / Infrastructure / API) en un solo proyecto, MediatR para despachar queries, FluentValidation para validar input y Dapper sobre PostgreSQL llamando a funciones SQL como equivalente de stored procedures.

## Estructura

```
backend/
├── TaskManager.sln
├── src/TaskManager.Api/
│   ├── API/
│   │   ├── Controller/         Endpoints HTTP (anémicos: solo mediator.Send)
│   │   └── Middleware/         Traduce excepciones a status codes
│   ├── Application/
│   │   ├── Query/              GetTasksQuery, GetTaskByIdQuery (IRequest)
│   │   ├── QueryHandlers/      Casos de uso — orquestan repo + service
│   │   ├── Dtos/               TaskListItemDto (liviano), TaskDetailDto (completo)
│   │   ├── Validators/         FluentValidation por query
│   │   ├── Behaviors/          ValidationBehavior — corre validators antes del handler
│   │   ├── Exceptions/         ValidationException
│   │   └── Services/           ITaskService — contrato de mapeo / integraciones
│   ├── Domain/
│   │   ├── Entities/           TaskItem + VOs (TaskStatus, TaskPriority)
│   │   ├── Repository/         ITaskRepository (contrato)
│   │   └── Exceptions/         NotFoundException (excepción de dominio, sin HTTP)
│   ├── Infrastructure/
│   │   ├── Persistence/        TaskRow — POCO que hidrata Dapper
│   │   ├── Repository/         TaskRepository (impl Dapper + Npgsql)
│   │   └── Service/            TaskService (impl ITaskService)
│   └── Program.cs              DI, MediatR, FluentValidation, middleware, Swagger
└── tests/TaskManager.UnitTests/
    ├── QueryHandlers/          Tests de los handlers con repo mockeado
    └── Validators/             Tests de las reglas de FluentValidation
```

## Por qué Clean Architecture + CQRS

Cada capa solo conoce a la que tiene debajo:

- **Domain**: el núcleo, no depende de nada. Define **qué** se necesita (`ITaskRepository`, entidades con invariantes).
- **Application**: define los casos de uso (queries + handlers), valida input y mapea a DTOs. Depende solo de Domain.
- **Infrastructure**: las implementaciones concretas (Dapper, Npgsql, integraciones externas). Depende de Domain — es quien provee lo que el dominio pide.
- **API**: transporte HTTP. Es el *composition root*: la única capa que conoce a todas las demás, porque acá se "atan los cables" entre interfaces e implementaciones.

CQRS está aplicado **solo en el lado Query** — la API es read-only. La estructura queda lista para sumar `Command/` y `CommandHandlers/` el día que entre un `POST /api/tasks`. La separación importa: un cambio en la lectura no tiene por qué tocar la escritura ni viceversa.

Como siguiente paso para un proyecto más grande, lo "nivel senior" es partir esto en 4 `.csproj` (`TaskManager.Domain`, `TaskManager.Application`, `TaskManager.Infrastructure`, `TaskManager.Api`): hoy las capas son una convención de carpetas; con csproj separados el compilador **fuerza** que Domain no pueda importar Infrastructure.

## Stack

- **MediatR 12**: despacha cada `IRequest` al `IRequestHandler` correspondiente. El controller no conoce handlers ni repositorios — solo manda la query al mediator. Pipeline behaviors permiten transversales (validación, logging, caching) sin tocar el handler.
- **FluentValidation 11**: reglas declarativas en `Application/Validators`. Un validator por query, se ejecutan automáticamente vía `ValidationBehavior` antes de que el handler vea el request.
- **Dapper 2 + Npgsql 8**: capa fina sobre ADO.NET, ideal para llamar funciones / SPs sin la fricción de un ORM.
- **Serilog 8**: logging estructurado y request logging (`UseSerilogRequestLogging`).
- **Swashbuckle**: Swagger UI con XML comments en Development.
- **xUnit + NSubstitute + FluentAssertions**: tests de handlers con repo mockeado y tests de validators con `TestValidate`.

## Validación

Vive en `Application/Validators` — es regla del caso de uso, no de HTTP. Cada query tiene su `AbstractValidator<TQuery>`:

```csharp
RuleFor(x => x.StatusId)
    .InclusiveBetween(1, 4)
    .When(x => x.StatusId.HasValue)
    .WithMessage("Invalid status. Must be between 1 and 4.");
```

El `ValidationBehavior` de MediatR los corre antes del handler. Si alguno falla, lanza `ValidationException` con un `Dictionary<string, string[]>` y el handler nunca llega a ejecutarse. El middleware la traduce a HTTP 400 con el detalle por campo.

Esto reemplaza al `if (status < 1 || status > 4) BadRequest(...)` que estaba inline en el controller. Beneficios: una sola fuente de verdad por regla, reglas componibles, mensajes consistentes y centralizados.

## Modelo de dominio

`TaskItem` es una entidad **rica**, no un POCO con setters públicos:

- Constructor con todos los invariantes obligatorios (título no vacío, id positivo, status y priority no nulos).
- Setters privados — nadie de afuera puede mutar el estado a mano.
- `Status` y `Priority` son value objects (`TaskStatus`, `TaskPriority`) que encapsulan `id + code + label`. En vez de un enum hardcodeado, la fuente de verdad es la tabla `task_status` / `task_priority`; soporta metadata futura (color, orden) sin tocar la entidad.

Dapper no hidrata `TaskItem` directamente. Hidrata `TaskRow` (POCO con setters públicos en Infrastructure/Persistence) y el repositorio mapea `TaskRow → TaskItem` antes de salir. Eso mantiene los detalles de la DB (snake_case, `short` vs `int`) fuera del dominio.

## DTOs: listado vs detalle

Dos DTOs separados, dos SQLs diferentes:

- **`TaskListItemDto`** (4 campos: `id`, `title`, `status`, `priority`): lo justo para mostrar una fila. No incluye descripción ni códigos — solo los labels para los badges. El SQL del repo proyecta solo las columnas necesarias.
- **`TaskDetailDto`** (7 campos): incluye `description` y los codes + labels completos. El detalle sí necesita todo.

Tener DTOs distintos deja el contrato explícito en Swagger/OpenAPI y permite evolucionarlos por separado (ej. detalle suma `createdAt`, listado suma `thumbnail`) sin filtrar cambios de un caso de uso al otro.

## Errores

Único lugar que traduce excepciones a HTTP: el `ExceptionMiddleware` en `API/Middleware`. El resto del código lanza excepciones expresivas sin saber qué status code generan.

Mapeo:
- **`ValidationException`** → 400 con el dict de errores por campo.
- **`NotFoundException`** → 404 con detalle.
- cualquier otra → 500 con mensaje genérico (el detalle solo al log).

```json
{
  "status": 404,
  "title": "Not found",
  "detail": "Task with id 99999 was not found.",
  "instance": "/api/tasks/99999"
}
```

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL 16 corriendo (ver [`../db/README.md`](../db/README.md))

## Setup

Configurar el archivo `.env` antes de levantar la API:

```powershell
cd src/TaskManager.Api
Copy-Item .env.example .env
# editar .env si las credenciales de Postgres son distintas
```

El `.env` queda fuera del repo (gitignored). El template está en `.env.example`. Para cargarlo se usa `DotNetEnv` (`Env.TraversePath().Load()` al arranque del proceso). Las variables se exponen como `IConfiguration` con la convención `Section__Key` (ej. `ConnectionStrings__Default`).

En entornos productivos las variables se setean directamente como env vars del proceso/container, sin necesidad del archivo.

```powershell
dotnet restore
dotnet build
dotnet test

cd src/TaskManager.Api
dotnet run
# http://localhost:5186
# Swagger: http://localhost:5186/swagger
```

## Endpoints

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/health` | Healthcheck simple. |
| GET | `/api/tasks` | Lista tareas (DTO liviano). Filtros opcionales: `?status=&priority=`. |
| GET | `/api/tasks/{id}` | Detalle completo. 404 si no existe. |
| GET | `/swagger` | Swagger UI (solo en Development). |

### IDs de catálogos

Status: `1=pending`, `2=in_progress`, `3=completed`, `4=cancelled`.
Priority: `1=low`, `2=medium`, `3=high`, `4=urgent`.

### Ejemplos

```bash
curl http://localhost:5186/api/tasks
curl "http://localhost:5186/api/tasks?status=2"
curl "http://localhost:5186/api/tasks?status=1&priority=4"
curl http://localhost:5186/api/tasks/3
curl http://localhost:5186/api/tasks/99999    # 404
curl "http://localhost:5186/api/tasks?status=9" # 400 con detalle por campo
```

## Flujo de un request

`GET /api/tasks?status=2` recorre:

1. **`TasksController`** (API) — bind de query params, construye `GetTasksQuery(2, null)` y se lo manda a `IMediator.Send`.
2. **`ValidationBehavior`** (Application/Behaviors) — corre `GetTasksQueryValidator`. Si falla, lanza `ValidationException` y nunca se llega al handler.
3. **`GetTasksQueryHandler`** (Application/QueryHandlers) — pide los datos a `ITaskRepository`, mapea entidades a DTOs con `ITaskService`.
4. **`TaskRepository`** (Infrastructure) — ejecuta el SQL contra Postgres con Dapper, mapea `TaskRow → TaskItem`.
5. El handler devuelve `IReadOnlyList<TaskListItemDto>` al mediator, el controller responde `200 OK`.

Si algo falla, el `ExceptionMiddleware` traduce la excepción al status correspondiente.

## Notas

- Mapeo snake_case → PascalCase configurado con `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true`. Hace que columnas como `status_id` mapeen a `StatusId` sin alias explícitos.
- MediatR y FluentValidation descubren handlers y validators por reflection sobre el assembly al arranque (`AddMediatR` y `AddValidatorsFromAssembly`). No hay que registrarlos uno por uno.
- Repo y service registrados como Scoped (instancia por request). Convención en .NET; deja la puerta abierta a inyectar dependencias con scope sin riesgo de *captive dependency*. La conexión real la maneja Npgsql con su pool, no la vida del objeto.
- `TaskStatus` (VO de dominio) colisiona en nombre con `System.Threading.Tasks.TaskStatus`. Se resuelve con alias (`using DomainTaskStatus = TaskManager.Api.Domain.Entities.TaskStatus;`) en los archivos donde hace falta.
