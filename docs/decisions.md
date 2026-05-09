# Decisiones técnicas

## Backend

Elegí **layered en un solo proyecto** en lugar de Clean Architecture + CQRS. El reto pide 2 endpoints: partir el código en 4 proyectos y meter MediatR para dos queries era complejidad innecesaria.

Para acceso a datos preferí **Dapper sobre EF Core** porque el reto pide stored procedures. Con Dapper ejecuto el SQL o llamo a la función, recibo POCOs mapeados, y listo. EF Core trae cosas (change-tracking, unit-of-work, migrations) que no se usan cuando el flujo es ejecutar un SP, mapear y devolver.

Las "stored procedures" son en realidad **funciones** (`CREATE FUNCTION ... RETURNS TABLE`). En Postgres los `CREATE PROCEDURE` no pueden retornar resultados tabulares, así que para el caso es lo correcto. Mantuve el prefijo `sp_` por convención del enunciado.

Para errores usé un **`ExceptionMiddleware` clásico** en lugar de `IExceptionHandler` con ProblemDetails. Lo elegí porque me resulta más fácil de leer y explicar: es básicamente lo mismo que un `app.use((err, req, res, next) => ...)` de Express.

Registré el **repositorio como Scoped** (instancia por request).Scoped además me deja inyectar otras dependencias con scope sin riesgo de *captive dependency*, cosa que con Singleton sí sería un problema.

Las **credenciales vienen de un `.env`**. Uso `DotNetEnv.Env.TraversePath().Load()` al arranque para cargar el archivo en variables de entorno. .NET las lee como `IConfiguration` con la convención `Section__Key` (por ejemplo `ConnectionStrings__Default`). En producción no se usa el archivo: las variables se setean directamente en el container.

## Mobile

Estructuré la app **por features con capas adentro de cada una** (`domain`, `data`, `application`, `ui`). Un cambio en `tasks` toca un solo folder, y cuando entren features nuevas (auth, profile) se duplica la estructura. Es la misma idea que Clean Architecture en el backend, más liviana.

Separé **containers (`screens/`) y vistas (`views/`)**. Las views son funciones puras de `props → JSX`, lo que me permite testearlas con `render(<View ...props />)` sin levantar Redux ni el navigator. Los screens orquestan hooks, dispatch y navegación. Lo malo es que termina habiendo doble archivo por pantalla, pero a la larga vale la pena cuando entran tests de UI.

Usé **Redux Toolkit + RTK Query** porque el reto lo pide. La ventaja real es que el estado del cliente y el del servidor viven en el mismo store, con un solo devtool. RTK Query maneja cache, deduplicación, refetch al recuperar foco / conexión y los estados de loading/error sin tener que escribirlos a mano. El `filtersSlice` solo guarda los filtros activos. Quise evitar cachear datos del servidor en un slice manualmente, que termina dando muchos problemas.

Para que el cache sobreviva al cierre de la app sumé **`redux-persist` con AsyncStorage**. Persisto `filters` y el cache de `tasksApi`. `extractRehydrationInfo` le dice a RTK Query cómo leer el state guardado al rehidratar. Si el volumen de datos creciera mucho migraría a MMKV (más rápido y más liviano). Para offline real haría falta una base de datos local, pero queda fuera del alcance del reto.

Mantuve un **DTO + Mapper** aunque hoy es prácticamente lo mismo. Lo dejé así para tener un lugar definido donde traducir entre el JSON del backend y el modelo de la app: si el backend renombra un campo, solo el mapper se toca.

Para no andar repitiendo colores y medidas centralicé los tokens en `core/theme/tokens.ts`. Tambien llamados globalStyles.

Para las URLs del backend usé **`react-native-dotenv`**. Las URLs viven en `mobile/.env` y se importan tipadas desde `@env`. Lo elegí por sobre `react-native-config` porque es solo un plugin de babel: no hay que tocar el código nativo (`build.gradle`, `Podfile`).

Cada pantalla con datos del servidor maneja **estados explícitos** (loading / empty / error / success) con UI distinta para cada uno.

## Database

Modelé los **estados y prioridades como tablas de referencia** (`task_status`, `task_priority`). Permite agregar metadata futura (color, orden, traducciones) sin alterar la tabla principal y mantiene integridad referencial. El ENUM nativo de Postgres es muy rígido: agregar valores requiere `ALTER TYPE` y borrarlos es complicado.

Usé **`BIGINT GENERATED ALWAYS AS IDENTITY`** para el ID de `tasks`.

Las fechas son **`TIMESTAMPTZ`** siempre. Postgres convierte a UTC al guardar y permite expresar zonas horarias en queries.

Agregué **índices** sobre `status_id`, `priority_id` y `created_at DESC`. Cubre los tres patrones de acceso esperados: filtros + orden por defecto.

## Lo que dejé afuera
- **Banner de modo offline**: requiere `@react-native-community/netinfo`. Sería el primer agregado si tuviera más tiempo.
- **DB local para offline-first**: solo se justifica si la app tiene que funcionar bien sin internet como funcionalidad principal (delivery, médicas, etc.). Para listado/detalle, AsyncStorage + RTK Query alcanza.
- **Paginación**: con 18 tareas en el seed no hace falta. Cuando crezca, sumo `LIMIT/OFFSET` a `sp_get_tasks` y `merge` en RTK Query para listas infinitas.
