import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { setupListeners } from '@reduxjs/toolkit/query';
import AsyncStorage from '@react-native-async-storage/async-storage';
import {
  FLUSH,
  PAUSE,
  PERSIST,
  PURGE,
  REGISTER,
  REHYDRATE,
  persistReducer,
  persistStore,
} from 'redux-persist';
import { tasksApi } from '../../features/tasks/data/tasksApi';
import { filtersReducer } from '../../features/tasks/application/filtersSlice';

const rootReducer = combineReducers({
  [tasksApi.reducerPath]: tasksApi.reducer,
  filters: filtersReducer,
});

/**
 * Persistimos:
 *  - `filters`: para que el usuario abra la app y mantenga sus filtros activos.
 *  - `tasksApi`: para que la última lista cargada sobreviva al cierre de la app
 *    (modo offline). RTK Query rehidrata el cache via `extractRehydrationInfo`.
 */
const persistConfig = {
  key: 'root',
  storage: AsyncStorage,
  whitelist: ['filters', tasksApi.reducerPath],
  version: 1,
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

export const store = configureStore({
  reducer: persistedReducer,
  middleware: (getDefault) =>
    getDefault({
      // Las acciones internas de redux-persist contienen funciones (no son
      // serializables); las excluimos del check para que no spammeen warnings.
      serializableCheck: {
        ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
      },
    }).concat(tasksApi.middleware),
});

export const persistor = persistStore(store);

// Habilita refetch on focus / reconnect (RTK Query)
setupListeners(store.dispatch);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
