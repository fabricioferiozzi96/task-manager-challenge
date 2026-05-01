-- ============================================================
-- Task Manager — Datos por defecto
-- ============================================================

-- Catálogos
INSERT INTO task_status (id, code, label, sort_order) VALUES
    (1, 'pending',     'Pendiente',    1),
    (2, 'in_progress', 'En progreso',  2),
    (3, 'completed',   'Completada',   3),
    (4, 'cancelled',   'Cancelada',    4);

INSERT INTO task_priority (id, code, label, weight) VALUES
    (1, 'low',    'Baja',    1),
    (2, 'medium', 'Media',   2),
    (3, 'high',   'Alta',    3),
    (4, 'urgent', 'Urgente', 4);

-- Tareas de prueba (variedad de combinaciones estado/prioridad)
INSERT INTO tasks (title, description, status_id, priority_id, due_date) VALUES
    ('Diseñar wireframes de la app',
     'Crear mockups iniciales de las pantallas principales: lista, detalle y filtros.',
     3, 3, '2026-04-15 18:00:00+00'),

    ('Configurar pipeline CI/CD',
     'Setup de GitHub Actions con build, tests y análisis estático para el backend y mobile.',
     2, 2, '2026-05-20 12:00:00+00'),

    ('Implementar autenticación OAuth',
     'Integrar login con Google y Apple. Considerar refresh tokens y almacenamiento seguro.',
     1, 4, '2026-05-30 23:59:00+00'),

    ('Refactorizar módulo de notificaciones',
     'Migrar de Firebase Cloud Messaging a un sistema agnóstico de proveedor.',
     1, 2, '2026-06-15 18:00:00+00'),

    ('Documentar API pública',
     'Generar OpenAPI completo con ejemplos y publicar en Swagger UI.',
     2, 1, '2026-05-25 18:00:00+00'),

    ('Optimizar queries de listado',
     'Análisis EXPLAIN de queries más usadas; agregar índices donde corresponda.',
     1, 3, '2026-06-01 18:00:00+00'),

    ('Migrar tests legacy a Jest 29',
     'Actualizar suite de tests del frontend a la última versión y arreglar breaking changes.',
     3, 1, '2026-04-01 18:00:00+00'),

    ('Agregar dark mode',
     'Tema oscuro para la app móvil con switch automático según preferencia del sistema.',
     1, 1, '2026-07-10 18:00:00+00'),

    ('Reunión de arquitectura Q3',
     'Planning de hitos técnicos del próximo trimestre con todo el equipo.',
     1, 4, '2026-05-08 14:00:00+00'),

    ('Resolver bug crítico en sync offline',
     'La cola de cambios se duplica al reconectar tras estar offline más de 1 hora.',
     2, 4, '2026-05-07 23:59:00+00'),

    ('Code review PR #142',
     'Revisión de la implementación de cache distribuido en el microservicio de inventario.',
     1, 2, '2026-05-09 18:00:00+00'),

    ('Actualizar dependencias mensuales',
     'Revisar Renovate bot, mergear PRs de patches y minor versions, validar tests.',
     2, 1, '2026-05-15 18:00:00+00'),

    ('Investigar caída de p95 latency',
     'El endpoint de búsqueda subió de 120ms a 380ms en la última semana.',
     2, 4, '2026-05-08 18:00:00+00'),

    ('Onboarding nuevo desarrollador',
     'Preparar setup del entorno y plan de aprendizaje del primer mes.',
     3, 2, '2026-04-20 18:00:00+00'),

    ('Postmortem incidente del 28/04',
     'Documentar root cause analysis del downtime de 22 minutos en producción.',
     3, 3, '2026-05-02 18:00:00+00'),

    ('Definir SLOs del servicio de pagos',
     'Setear objetivos de availability y latency, agregar dashboards en Grafana.',
     1, 3, '2026-06-30 18:00:00+00'),

    ('Limpiar feature flags obsoletos',
     'Identificar y remover toggles que llevan más de 6 meses al 100%.',
     4, 1, '2026-04-30 18:00:00+00'),

    ('Configurar alertas de errores en Sentry',
     'Definir umbrales por severidad y rutas de escalado a oncall.',
     1, 2, '2026-05-22 18:00:00+00');
