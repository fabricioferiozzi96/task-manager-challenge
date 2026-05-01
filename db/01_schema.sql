-- ============================================================
-- Task Manager — Schema
-- PostgreSQL 16
-- ============================================================
-- Diseño:
--   * Estados y prioridades como tablas de referencia (no enums nativos
--     ni CHECK constraints) para permitir agregar metadata futura
--     (orden, color, traducciones) sin alterar la tabla principal.
--   * IDs BIGINT IDENTITY para tasks: simple, eficiente, suficiente
--     para el alcance del reto.
--   * Timestamps con TIMESTAMPTZ para evitar ambigüedades de zona horaria.
-- ============================================================

DROP TABLE IF EXISTS tasks CASCADE;
DROP TABLE IF EXISTS task_status CASCADE;
DROP TABLE IF EXISTS task_priority CASCADE;

CREATE TABLE task_status (
    id          SMALLINT     PRIMARY KEY,
    code        VARCHAR(20)  NOT NULL UNIQUE,
    label       VARCHAR(50)  NOT NULL,
    sort_order  SMALLINT     NOT NULL
);

COMMENT ON TABLE task_status IS 'Catálogo de estados posibles de una tarea';

CREATE TABLE task_priority (
    id      SMALLINT     PRIMARY KEY,
    code    VARCHAR(20)  NOT NULL UNIQUE,
    label   VARCHAR(50)  NOT NULL,
    weight  SMALLINT     NOT NULL
);

COMMENT ON TABLE task_priority IS 'Catálogo de prioridades. weight permite ordenar de mayor a menor.';

CREATE TABLE tasks (
    id           BIGINT       GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title        VARCHAR(200) NOT NULL,
    description  TEXT,
    status_id    SMALLINT     NOT NULL REFERENCES task_status(id),
    priority_id  SMALLINT     NOT NULL REFERENCES task_priority(id),
    due_date     TIMESTAMPTZ,
    created_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW()
);

COMMENT ON TABLE tasks IS 'Tareas del usuario';

-- Índices para los filtros más comunes (status y priority)
CREATE INDEX idx_tasks_status   ON tasks(status_id);
CREATE INDEX idx_tasks_priority ON tasks(priority_id);
CREATE INDEX idx_tasks_created  ON tasks(created_at DESC);
