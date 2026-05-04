import { configureStore } from '@reduxjs/toolkit';
import { setupListeners } from '@reduxjs/toolkit/query';
import { tasksApi } from '../../features/tasks/data/tasksApi';
import { filtersReducer } from '../../features/tasks/application/filtersSlice';

export const store = configureStore({
  reducer: {
    [tasksApi.reducerPath]: tasksApi.reducer,
    filters: filtersReducer,
  },
  middleware: (getDefault) => getDefault().concat(tasksApi.middleware),
});

// Habilita refetch on focus / reconnect (RTK Query)
setupListeners(store.dispatch);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
