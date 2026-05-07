# Task Manager

Mini app de gestión de tareas: listado, filtrado y detalle de tareas personales.

Stack: React Native CLI (TypeScript) + .NET 8 Web API + PostgreSQL 16.

## Estructura

```
task-manager-challenge/
├── backend/   API .NET 8
├── mobile/    App React Native CLI
├── db/        Scripts SQL: schema, seed, funciones
└── docs/      Notas y diagramas
```

## Cómo correrlo

1. Configurar la base de datos: ver [`db/README.md`](db/README.md).
2. Levantar el backend: [`backend/README.md`](backend/README.md).
3. Correr la app móvil: [`mobile/README.md`](mobile/README.md).

Las decisiones técnicas relevantes están explicadas en el README de cada subcarpeta.

## Convenciones

- Commits siguen [Conventional Commits](https://www.conventionalcommits.org/) (`feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`).
- Branch `main` siempre estable.
