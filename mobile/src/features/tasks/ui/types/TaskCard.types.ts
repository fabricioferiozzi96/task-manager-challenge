import type { Task } from '../../domain/Task';

export interface TaskCardProps {
  task: Task;
  onPress: (id: number) => void;
}
