import React from 'react';
import { useGetTaskByIdQuery } from '../../data/tasksApi';
import { TaskDetailView } from '../views/TaskDetailView';
import type { RootStackProps } from '../../../../core/navigation/types';

/**
 * Container de detalle de tarea.
 */
export const TaskDetailScreen: React.FC<RootStackProps<'TaskDetail'>> = ({ route }) => {
  const { taskId } = route.params;
  const { data, isLoading, isError, refetch } = useGetTaskByIdQuery(taskId);

  return (
    <TaskDetailView
      task={data}
      isLoading={isLoading}
      isError={isError}
      onRetry={refetch}
    />
  );
};
