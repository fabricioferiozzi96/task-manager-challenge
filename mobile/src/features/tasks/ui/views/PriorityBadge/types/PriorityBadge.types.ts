import type { TaskPriority } from '../../../../domain/Task';

export interface PriorityBadgeProps {
  priority: TaskPriority;
  label: string;
  variant?: 'solid' | 'soft';
}
