# Backend - Task Manager API

API REST en .NET 8 con arquitectura layered clásica (Controller → Service → Repository) en un solo proyecto. Acceso a datos vía Dapper sobre PostgreSQL, llamando a funciones SQL como equivalente de stored procedures.

## Estructura

```
backend/
├── TaskManager.sln
├── src/TaskManager.Api/
│   ├── Controllers/      Endpoints HTTP
│   ├── Services/         Lógica de negocio (orquesta repo, mapea a DTO)
│   ├── Repositories/     Acceso a datos vía Dapper
│   ├── Models/           POCOs que mapean lo que devuelve la DB
│   ├── Dtos/             Forma del JSON expuesto al cliente
│   ├── Exceptions/       NotFoundException
│   ├── Middleware/       Manejo global de errores
│   └── Program.cs        DI, middleware, Serilog, Swagger, CORS
└── tests/TaskManager.UnitTests/
    └── Services/         Tests del TaskService con repositorio mockeado
```

## Por qué layered y no Clean Architecture

El reto pide 2 endpoints. Para ese alcance, partir el código en 4 proyectos (Domain / Application / Infrastructure / Api), agregar MediatR para 2 queries y modelar value objects para los enums es ceremonia que no aporta valor real.

Layered con Controller → Service → Repository ya separa lo importante:

- **Controller**: HTTP (binding, status codes).
- **Service**: lógica de negocio y mapeo a DTO.
- **Repository**: acceso a datos.

Si el dominio creciera (varios agregados, comandos con efectos secundarios, reglas de negocio complejas), Clean Arch + CQRS pasa a tener sentido. Para este alcance no.

## Stack

- **Dapper 2 + Npgsql 8**: capa fina sobre ADO.NET, ideal para llamar funciones / SPs sin la fricción de un ORM.
- **Serilog 8**: logging estructurado y request logging (`UseSerilogRequestLogging`).
- **Swashbuckle**: Swagger UI con XML comments en Development.
- **xUnit + NSubstitute + FluentAssertions**: tests del Service con repo mockeado.

Validación: la dejé inline en el controller. Son 2 query params numéricos en rango 1..4 y no me pareció justificado meter FluentValidation.

Errores: middleware clásico (`app.UseMiddleware<ExceptionMiddleware>()`) que mapea excepciones a JSON. Cumple el mismo rol que un `app.use((err, req, res, next) => ...)` en Express. Preferí esto antes que `IExceptionHandler` con ProblemDetails porque me resultó más directo de leer y explicar.

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
| GET | `/api/tasks` | Lista tareas. Filtros opcionales: `?status=&priority=`. |
| GET | `/api/tasks/{id}` | Detalle. 404 si no existe. |
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
```

## Manejo de errores

Todas las excepciones pasan por `ExceptionMiddleware` y se traducen a JSON consistente:

```json
{
  "status": 404,
  "title": "Not found",
  "detail": "Task with id 99999 was not found.",
  "instance": "/api/tasks/99999"
}
```

Casos:

- **400** cuando la validación inline en el controller falla (`status` o `priority` fuera de 1..4).
- **404** cuando el service lanza `NotFoundException`.
- **500** para cualquier otra excepción (se loguea con stack trace).

## Notas

- Mapeo snake_case → PascalCase configurado con `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true`. Hace que columnas como `status_id` mapeen a `StatusId` sin alias explícitos.
- El repositorio está registrado como Singleton. No guarda estado: abre y cierra una `NpgsqlConnection` por llamada, y Npgsql tiene su propio pool interno. Scoped no aporta nada acá.
