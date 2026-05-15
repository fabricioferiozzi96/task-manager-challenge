import { Badge } from '../../../../../shared/components/Badge';
import { STATUS_COLOR } from './constants/statusColors';
import type { StatusBadgeProps } from './types/StatusBadge.types';

export const StatusBadge = ({ status, label }: StatusBadgeProps) => (
  <Badge label={label} color={STATUS_COLOR[status]} />
);
