import type { Task } from '../../domain/Task';

export interface TaskDetailViewProps {
  task: Task | undefined;
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
}
