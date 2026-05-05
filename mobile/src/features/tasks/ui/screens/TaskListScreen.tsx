import React, { useCallback, useMemo } from 'react';
import { useGetTasksQuery } from '../../data/tasksApi';
import { useAppDispatch, useAppSelector } from '../../../../core/store/hooks';
import { clearFilters } from '../../application/filtersSlice';
import {
  PRIORITY_OPTIONS,
  STATUS_OPTIONS,
} from '../../domain/Task';
import { TaskListView } from '../views/TaskListView';
import type { RootStackProps } from '../../../../core/navigation/types';

/**
 * Container de la lista de tareas.
 * Solo conecta Redux + RTK Query + navegación con la vista presentacional.
 */
export const TaskListScreen: React.FC<RootStackProps<'TaskList'>> = ({ navigation }) => {
  const filters = useAppSelector((s) => s.filters);
  const dispatch = useAppDispatch();
  const { data, isLoading, isError, refetch, isFetching } = useGetTasksQuery(filters);

  const handleTaskPress = useCallback(
    (id: number) => navigation.navigate('TaskDetail', { taskId: id }),
    [navigation],
  );

  const handleFilterPress = useCallback(
    () => navigation.navigate('Filters'),
    [navigation],
  );

  const handleClearFilters = useCallback(
    () => dispatch(clearFilters()),
    [dispatch],
  );

  const activeFilterLabels = useMemo(() => {
    const labels: string[] = [];
    if (filters.status !== undefined) {
      const opt = STATUS_OPTIONS.find((s) => s.id === filters.status);
      if (opt) labels.push(opt.label);
    }
    if (filters.priority !== undefined) {
      const opt = PRIORITY_OPTIONS.find((p) => p.id === filters.priority);
      if (opt) labels.push(opt.label);
    }
    return labels;
  }, [filters]);

  return (
    <TaskListView
      tasks={data}
      isLoading={isLoading}
      isError={isError}
      isFetching={isFetching}
      activeFilterLabels={activeFilterLabels}
      onTaskPress={handleTaskPress}
      onFilterPress={handleFilterPress}
      onClearFilters={handleClearFilters}
      onRetry={refetch}
      onRefresh={refetch}
    />
  );
};
