import type { Task } from '../../../../domain/Task';

export interface TaskListViewProps {
  tasks: Task[] | undefined;
  isLoading: boolean;
  isError: boolean;
  isFetching: boolean;
  activeFilterLabels: string[];
  onTaskPress: (id: number) => void;
  onFilterPress: () => void;
  onClearFilters: () => void;
  onRetry: () => void;
  onRefresh: () => void;
}
