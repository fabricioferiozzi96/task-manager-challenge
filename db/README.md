# Database - PostgreSQL 16

Schema, datos por defecto y "stored procedures" para la app de gestión de tareas.

## Decisiones de diseño

- **Estados y prioridades como tablas de referencia** (`task_status`, `task_priority`) en lugar de enums nativos o `CHECK` constraints. Permite agregar metadata futura (orden visual, colores, traducciones) sin alterar la tabla principal y mantiene integridad referencial.
- **`BIGINT GENERATED ALWAYS AS IDENTITY`** para el PK de `tasks`. Sintaxis SQL estándar moderna, suficiente para el alcance.
- **`TIMESTAMPTZ`** en `due_date`, `created_at` y `updated_at` para evitar ambigüedades de zona horaria al exponer datos a clientes móviles.
- **Funciones en lugar de procedures.** En PostgreSQL los `CREATE PROCEDURE` no pueden retornar result sets directamente. Para cumplir con el "stored procedure" que pide el reto, lo idiomático es `CREATE FUNCTION ... RETURNS TABLE`. Mantengo el prefijo `sp_` en el nombre por convención.
- **Índices** sobre `status_id`, `priority_id` y `created_at DESC`. Cubre los tres patrones de acceso esperados (filtros + orden por defecto).

## Schema

```
task_status   ───┐
                 ├──< tasks
task_priority ───┘
```

- `task_status`: catálogo de estados (Pendiente, En progreso, Completada, Cancelada).
- `task_priority`: catálogo de prioridades (Baja, Media, Alta, Urgente).
- `tasks`: tareas del usuario.

## Funciones expuestas

- `sp_get_tasks(p_status_id, p_priority_id)`: lista tareas. `NULL` en cualquier parámetro = sin filtro.
- `sp_get_task_by_id(p_id)`: detalle de una tarea, 0 filas si no existe.

Las dos retornan ya joineadas las descripciones humanas (`status_label`, `priority_label`) para que la API no tenga que hacer JOINs adicionales.

## Setup

Pre-requisitos:

- PostgreSQL 16+ corriendo en `localhost:5432`.
- Usuario con permisos para crear bases de datos (en este reto, `postgres`).

Desde la raíz del repo, en PowerShell:

```powershell
$env:PGPASSWORD = "postgres"   # ajustar según el entorno
$psql = "C:\Program Files\PostgreSQL\16\bin\psql.exe"

& $psql -U postgres -h localhost -d postgres -c "DROP DATABASE IF EXISTS taskmanager;"
& $psql -U postgres -h localhost -d postgres -c "CREATE DATABASE taskmanager;"
& $psql -U postgres -h localhost -d taskmanager -v ON_ERROR_STOP=1 -f db/01_schema.sql
& $psql -U postgres -h localhost -d taskmanager -v ON_ERROR_STOP=1 -f db/02_seed.sql
& $psql -U postgres -h localhost -d taskmanager -v ON_ERROR_STOP=1 -f db/03_functions.sql
```

Verificación:

```sql
-- 18 tareas
SELECT COUNT(*) FROM tasks;

-- Filtro por estado=2 (En progreso) y prioridad=4 (Urgente)
SELECT id, title, status_label, priority_label
FROM sp_get_tasks(2::SMALLINT, 4::SMALLINT);

-- Detalle por ID
SELECT * FROM sp_get_task_by_id(3);
```

## Connection string

Para usar desde el backend:

```
Host=localhost;Port=5432;Database=taskmanager;Username=postgres;Password=postgres
```

En entornos compartidos no usar la contraseña por defecto. Para este reto local quedó `postgres` (el winget la setea automáticamente al instalar).
