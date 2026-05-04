import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { API_BASE_URL } from '../../../core/api/config';
import type { Task, TaskFilters } from '../domain/Task';
import type { TaskDto } from './TaskDto';
import { toTask } from './TaskMapper';

/**
 * Fuente de verdad del server-state para tasks.
 * RTK Query maneja cache, dedupe, refetch y estados de loading/error de forma declarativa.
 */
export const tasksApi = createApi({
  reducerPath: 'tasksApi',
  baseQuery: fetchBaseQuery({ baseUrl: API_BASE_URL }),
  tagTypes: ['Task'],
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
