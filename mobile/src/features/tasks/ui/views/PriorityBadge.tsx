import React from 'react';
import { Badge } from '../../../../shared/components/Badge';
import { colors } from '../../../../core/theme/tokens';
import type { TaskPriority } from '../../domain/Task';

const PRIORITY_COLOR: Record<TaskPriority, string> = {
  low: colors.priority.low,
  medium: colors.priority.medium,
  high: colors.priority.high,
  urgent: colors.priority.urgent,
};

interface Props {
  priority: TaskPriority;
  label: string;
  variant?: 'solid' | 'soft';
}

export const PriorityBadge: React.FC<Props> = ({ priority, label, variant }) => (
  <Badge label={label} color={PRIORITY_COLOR[priority]} variant={variant} />
);
