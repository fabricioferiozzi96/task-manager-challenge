# Mobile - Task Manager App

App móvil en React Native CLI (TypeScript estricto) con Redux Toolkit + RTK Query y React Navigation.

## Estructura

Feature-sliced con capas adentro de cada feature.

```
mobile/src/
├── core/                       Infraestructura compartida
│   ├── api/                    URL base por plataforma (10.0.2.2 Android, localhost iOS)
│   ├── store/                  configureStore + hooks tipados
│   ├── navigation/             RootNavigator + RootStackParamList tipado
│   └── theme/                  Tokens (colors, spacing, typography, radius)
├── shared/
│   └── components/             Badge, StateMessage (loading/empty/error)
└── features/
    └── tasks/
        ├── domain/             Task entity + enums + opciones para UI
        ├── data/               TaskDto, TaskMapper, tasksApi (RTK Query)
        ├── application/        filtersSlice (client-state)
        └── ui/
            ├── screens/        Containers: hooks, dispatch, navegación
            └── views/          Vistas presentacionales puras (props → JSX)
```

## Por qué esta estructura

- **Feature-sliced**: un cambio en `tasks` toca un solo folder. Cuando entren nuevas features (auth, profile), se duplica la estructura adentro de `features/`.
- **Capas dentro de la feature**: misma idea que Clean Architecture en el backend. `domain/` no importa nada externo (ni React, ni Redux, ni axios), así que se puede testear con mocks puros.
- **Containers vs presentacionales**: los `screens/` orquestan hooks, dispatch y navegación, pero no tienen JSX propio (arman props y delegan en su `View`). Las `views/` son funciones puras de `props → JSX`. Esto permite testear las views con `render(<View ...props />)` sin levantar el store ni el navigator.
- **Server-state separado de client-state**:
  - **RTK Query** maneja lo que viene del servidor (cache, dedupe, refetch, loading/error).
  - **filtersSlice** guarda los filtros activos (UI state).
  - Esto evita el clásico antipatrón de "cachear datos del server en un slice de Redux a mano", que termina siendo bug magnet.

## Stack

- **TypeScript 5.8** en modo strict.
- **Redux Toolkit + RTK Query** para estado cliente y servidor en un mismo store.
- **React Navigation** con stack nativo y tipado.
- **`fetchBaseQuery`** (parte de RTK Query) para HTTP.
- **StyleSheet nativo** + tokens. Sin UI Kit (es restricción del reto).
- **Jest + React Native Testing Library** para tests unitarios.

## Requisitos

- Node.js 22+ (instalado)
- JDK 17 (instalado)
- Android Studio + Android SDK + emulador
- Backend corriendo en `http://localhost:5186` (ver [`../backend/README.md`](../backend/README.md))

## Setup de Android (primera vez)

Si nunca usaste Android Studio en este equipo:

1. Abrir Android Studio y completar el wizard "Standard install" (descarga SDK + tools, ~5-10 min).
2. **Tools → Device Manager → Create Device**: elegir Pixel 6 con system image API 34 (Android 14).
3. Verificar que `ANDROID_HOME` apunte a `C:\Users\<user>\AppData\Local\Android\Sdk` (Android Studio normalmente lo setea solo).
4. Agregar al `Path` del usuario: `%ANDROID_HOME%\platform-tools` y `%ANDROID_HOME%\emulator`.
5. Verificar abriendo nueva PowerShell: `adb --version` debería responder.

## Correr la app

Antes del primer build, configurar el `.env`:

```powershell
cd mobile
Copy-Item .env.example .env
# ajustar API_URL_ANDROID / API_URL_IOS si el backend no corre en el host default
npm install
```

Luego:

```powershell
# 1. Levantar emulador
emulator -avd Pixel_6_API_34   # o desde Android Studio → Device Manager → ▶

# 2. (otra terminal) Iniciar Metro
cd mobile
npm start

# 3. (otra terminal) Build y deploy
cd mobile
npm run android
```

## URL del backend

Las URLs viven en `mobile/.env` (gitignored, template en `.env.example`). Se cargan vía `react-native-dotenv` y se exponen tipadas en `src/env.d.ts`. `src/core/api/config.ts` elige cuál usar según la plataforma:

- **Android emulator**: `10.0.2.2` (alias del emulador para `localhost` del host).
- **iOS Simulator**: `localhost` (el simulador comparte red con la Mac).
- **Device físico**: ajustar a la IP de la máquina en la red local.

> Si después de modificar `.env` los cambios no toman, limpiar la cache de Metro: `npm start -- --reset-cache`. Babel cachea la transformación del plugin.

## Decisiones técnicas

### Redux Toolkit + RTK Query

Pedido explícito del reto. La ventaja real es que client-state y server-state viven en el mismo store: un solo devtool, un solo modelo mental. RTK Query maneja cache, dedupe, refetch on focus / reconnect (con `setupListeners`) y los estados loading/error organizados, sin tener que escribirlos a mano con thunks.

### DTO + Mapper

`TaskDto` es la forma exacta del JSON de la API. `TaskMapper.toTask` convierte DTO → entidad de dominio. Hoy es casi identidad porque el backend devuelve la forma que la app necesita, pero lo dejé así para tener un seam claro: si el backend renombra un campo, solo el mapper se toca.

### Domain agnóstico

`domain/Task.ts` no importa nada externo. Es un módulo de tipos puros y constantes (los IDs de los catálogos). Eso permite que cualquier consumidor lo use sin arrastrar dependencias.

### Sin UI Kit

Restricción del reto. Para no terminar hardcodeando colores y medidas en cada componente, centralicé tokens en `core/theme/tokens.ts`. Si más adelante hay que sumar dark mode, los tokens son el único lugar a tocar.

### Sin path aliases

Path aliases vía `babel-plugin-module-resolver` requiere mantener sincronizadas 4 configs (TS, Babel, Jest, Metro). Para un proyecto chico no compensa la fricción. Los `../../` quedan, pero son ruido tolerable. En un proyecto más grande sí los agregaría.

### Estados explícitos

Todas las pantallas con datos del servidor manejan loading / empty / error / success de forma visualmente distinta. Sin "renderizar nada en silencio mientras carga".

## Performance

- `TaskCard` está envuelto en `React.memo` para que la lista no re-renderice todas las cards cuando cambia algo arriba.
- `FlatList` con `keyExtractor` y `renderItem` memoizado con `useCallback`.
- No usé `getItemLayout` porque las cards tienen altura variable (la descripción es opcional). Si las cards fueran de altura fija, sí lo agregaría.

## Tests

Tres unidades:

- `TaskMapper.test.ts`: traducción DTO → Task.
- `filtersSlice.test.ts`: reducers (setStatus, setPriority, clearFilters).
- `TaskCard.test.tsx`: render + interacción con RNTL.

No testeo containers (los `screens/`) porque son glue (hooks de Redux + navigation). Mockear todo eso para tests unitarios es mucha fricción para poco valor. El valor real está en la lógica pura: mapper, reducers, views.

```powershell
npm test
```

## Troubleshooting

| Síntoma | Causa probable | Fix |
| --- | --- | --- |
| `Unable to resolve module ...` | Cache de Metro vieja | `npm start -- --reset-cache` |
| `Network request failed` en Android | URL hardcodeada `localhost` | Verificar que `config.ts` use `10.0.2.2` para Android |
| `SDK location not found` | `ANDROID_HOME` no seteado | Setear var de entorno o crear `android/local.properties` |
| Build Android lentísimo | Defender escaneando node_modules | Excluir `node_modules` y `android/.gradle` del Defender |
