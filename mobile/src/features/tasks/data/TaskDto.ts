import type { TaskPriority, TaskStatus } from '../domain/Task';

/**
 * Forma exacta del JSON que retorna la API.
 * No usar fuera de la capa data — el resto de la app consume `Task`.
 */
export interface TaskDto {
  id: number;
  title: string;
  description: string | null;
  status: TaskStatus;
  statusLabel: string;
  priority: TaskPriority;
  priorityLabel: string;
}
