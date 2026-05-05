import React, { useCallback } from 'react';
import { useAppDispatch, useAppSelector } from '../../../../core/store/hooks';
import {
  clearFilters,
  setPriority,
  setStatus,
} from '../../application/filtersSlice';
import { FiltersView } from '../views/FiltersView';
import type { RootStackProps } from '../../../../core/navigation/types';

/**
 * Container de filtros.
 */
export const FiltersScreen: React.FC<RootStackProps<'Filters'>> = ({ navigation }) => {
  const filters = useAppSelector((s) => s.filters);
  const dispatch = useAppDispatch();

  const handleToggleStatus = useCallback(
    (id: number | undefined) => dispatch(setStatus(id)),
    [dispatch],
  );

  const handleTogglePriority = useCallback(
    (id: number | undefined) => dispatch(setPriority(id)),
    [dispatch],
  );

  const handleClear = useCallback(() => dispatch(clearFilters()), [dispatch]);

  const handleApply = useCallback(() => navigation.goBack(), [navigation]);

  return (
    <FiltersView
      selectedStatusId={filters.status}
      selectedPriorityId={filters.priority}
      onToggleStatus={handleToggleStatus}
      onTogglePriority={handleTogglePriority}
      onClear={handleClear}
      onApply={handleApply}
    />
  );
};
