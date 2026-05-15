import { Badge } from '../../../../../shared/components/Badge';
import { PRIORITY_COLOR } from './constants/priorityColors';
import type { PriorityBadgeProps } from './types/PriorityBadge.types';

export const PriorityBadge = ({ priority, label, variant }: PriorityBadgeProps) => (
  <Badge label={label} color={PRIORITY_COLOR[priority]} variant={variant} />
);
