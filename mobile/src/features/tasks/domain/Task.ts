/**
 * Modelo de dominio puro. No conoce de JSON, HTTP ni Redux.
 */

export type TaskStatus = 'pending' | 'in_progress' | 'completed' | 'cancelled';
export type TaskPriority = 'low' | 'medium' | 'high' | 'urgent';

export interface Task {
  id: number;
  title: string;
  description: string | null;
  status: TaskStatus;
  statusLabel: string;
  priority: TaskPriority;
  priorityLabel: string;
}

export interface TaskFilters {
  status?: number;
  priority?: number;
}

// IDs de los catálogos en la DB. Mantienen sincronía con sp_get_tasks.
export const STATUS_IDS: Record<TaskStatus, number> = {
  pending: 1,
  in_progress: 2,
  completed: 3,
  cancelled: 4,
};

export const PRIORITY_IDS: Record<TaskPriority, number> = {
  low: 1,
  medium: 2,
  high: 3,
  urgent: 4,
};

export const STATUS_OPTIONS: { id: number; slug: TaskStatus; label: string }[] = [
  { id: 1, slug: 'pending', label: 'Pendiente' },
  { id: 2, slug: 'in_progress', label: 'En progreso' },
  { id: 3, slug: 'completed', label: 'Completada' },
  { id: 4, slug: 'cancelled', label: 'Cancelada' },
];

export const PRIORITY_OPTIONS: { id: number; slug: TaskPriority; label: string }[] = [
  { id: 1, slug: 'low', label: 'Baja' },
  { id: 2, slug: 'medium', label: 'Media' },
  { id: 3, slug: 'high', label: 'Alta' },
  { id: 4, slug: 'urgent', label: 'Urgente' },
];
