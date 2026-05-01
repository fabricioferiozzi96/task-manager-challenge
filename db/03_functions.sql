-- ============================================================
-- Task Manager — "Stored procedures"
-- ============================================================
-- Nota arquitectónica:
--   En PostgreSQL, los CREATE PROCEDURE no pueden retornar result
--   sets directamente; el equivalente idiomático para SPs que
--   retornan datos tabulares son las FUNCTIONS con RETURNS TABLE.
--   Mantenemos el prefijo "sp_" en el nombre para conservar la
--   intención semántica de "stored procedure" del enunciado.
-- ============================================================

-- ------------------------------------------------------------
-- sp_get_tasks
--   Lista tareas con filtros opcionales por estado y prioridad.
--   Retorna también las descripciones humanas de los catálogos
--   para evitar JOINs adicionales del lado de la API.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_get_tasks(
    p_status_id   SMALLINT DEFAULT NULL,
    p_priority_id SMALLINT DEFAULT NULL
)
RETURNS TABLE (
    id              BIGINT,
    title           VARCHAR,
    description     TEXT,
    status_id       SMALLINT,
    status_code     VARCHAR,
    status_label    VARCHAR,
    priority_id     SMALLINT,
    priority_code   VARCHAR,
    priority_label  VARCHAR,
    due_date        TIMESTAMPTZ,
    created_at      TIMESTAMPTZ,
    updated_at      TIMESTAMPTZ
)
LANGUAGE sql
STABLE
AS $$
    SELECT
        t.id,
        t.title,
        t.description,
        t.status_id,
        s.code  AS status_code,
        s.label AS status_label,
        t.priority_id,
        p.code  AS priority_code,
        p.label AS priority_label,
        t.due_date,
        t.created_at,
        t.updated_at
    FROM tasks t
    INNER JOIN task_status   s ON s.id = t.status_id
    INNER JOIN task_priority p ON p.id = t.priority_id
    WHERE (p_status_id   IS NULL OR t.status_id   = p_status_id)
      AND (p_priority_id IS NULL OR t.priority_id = p_priority_id)
    ORDER BY p.weight DESC, t.created_at DESC;
$$;

COMMENT ON FUNCTION sp_get_tasks IS
    'Listado de tareas con filtros opcionales por estado y prioridad. NULL = sin filtro.';


-- ------------------------------------------------------------
-- sp_get_task_by_id
--   Retorna el detalle de una tarea, o ninguna fila si no existe.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_get_task_by_id(p_id BIGINT)
RETURNS TABLE (
    id              BIGINT,
    title           VARCHAR,
    description     TEXT,
    status_id       SMALLINT,
    status_code     VARCHAR,
    status_label    VARCHAR,
    priority_id     SMALLINT,
    priority_code   VARCHAR,
    priority_label  VARCHAR,
    due_date        TIMESTAMPTZ,
    created_at      TIMESTAMPTZ,
    updated_at      TIMESTAMPTZ
)
LANGUAGE sql
STABLE
AS $$
    SELECT
        t.id,
        t.title,
        t.description,
        t.status_id,
        s.code  AS status_code,
        s.label AS status_label,
        t.priority_id,
        p.code  AS priority_code,
        p.label AS priority_label,
        t.due_date,
        t.created_at,
        t.updated_at
    FROM tasks t
    INNER JOIN task_status   s ON s.id = t.status_id
    INNER JOIN task_priority p ON p.id = t.priority_id
    WHERE t.id = p_id;
$$;

COMMENT ON FUNCTION sp_get_task_by_id IS
    'Detalle de una tarea por ID. Retorna 0 filas si no existe.';
