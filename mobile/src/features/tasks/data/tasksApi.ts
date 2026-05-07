import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { REHYDRATE } from 'redux-persist';
import type { Action } from '@reduxjs/toolkit';
import { API_BASE_URL } from '../../../core/api/config';
import type { Task, TaskFilters } from '../domain/Task';
import type { TaskDto } from './TaskDto';
import { toTask } from './TaskMapper';

interface RehydrateAction extends Action<typeof REHYDRATE> {
  payload?: Record<string, unknown>;
  key: string;
}

const isRehydrate = (action: Action): action is RehydrateAction =>
  action.type === REHYDRATE;

/**
 * Fuente de verdad del server-state para tasks.
 * RTK Query maneja cache, dedupe, refetch y estados de loading/error de forma declarativa.
 *
 * El cache se persiste con redux-persist (ver core/store). `extractRehydrationInfo`
 * le indica a RTK Query cómo leer el state rehidratado al abrir la app.
 */
export const tasksApi = createApi({
  reducerPath: 'tasksApi',
  baseQuery: fetchBaseQuery({ baseUrl: API_BASE_URL }),
  tagTypes: ['Task'],
  refetchOnFocus: true,
  refetchOnReconnect: true,
  extractRehydrationInfo(action, { reducerPath }) {
    if (isRehydrate(action) && action.payload) {
      return action.payload[reducerPath] as ReturnType<typeof tasksApi.reducer>;
    }
  },
  endpoints: (build) => ({
    getTasks: build.query<Task[], TaskFilters>({
      query: (filters) => ({
        url: '/tasks',
        params: {
          ...(filters.status !== undefined && { status: filters.status }),
          ...(filters.priority !== undefined && { priority: filters.priority }),
        },
      }),
      transformResponse: (raw: TaskDto[]) => raw.map(toTask),
      providesTags: (result) =>
        result
          ? [
              ...result.map(({ id }) => ({ type: 'Task' as const, id })),
              { type: 'Task', id: 'LIST' },
            ]
          : [{ type: 'Task', id: 'LIST' }],
    }),

    getTaskById: build.query<Task, number>({
      query: (id) => `/tasks/${id}`,
      transformResponse: (raw: TaskDto) => toTask(raw),
      providesTags: (_r, _e, id) => [{ type: 'Task', id }],
    }),
  }),
});

export const { useGetTasksQuery, useGetTaskByIdQuery } = tasksApi;
