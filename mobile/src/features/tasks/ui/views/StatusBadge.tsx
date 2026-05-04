import React from 'react';
import { Badge } from '../../../../shared/components/Badge';
import { colors } from '../../../../core/theme/tokens';
import type { TaskStatus } from '../../domain/Task';

const STATUS_COLOR: Record<TaskStatus, string> = {
  pending: colors.status.pending,
  in_progress: colors.status.inProgress,
  completed: colors.status.completed,
  cancelled: colors.status.cancelled,
};

interface Props {
  status: TaskStatus;
  label: string;
}

export const StatusBadge: React.FC<Props> = ({ status, label }) => (
  <Badge label={label} color={STATUS_COLOR[status]} />
);
