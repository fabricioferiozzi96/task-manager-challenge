import type { TaskStatus } from '../../../../domain/Task';

export interface StatusBadgeProps {
  status: TaskStatus;
  label: string;
}
