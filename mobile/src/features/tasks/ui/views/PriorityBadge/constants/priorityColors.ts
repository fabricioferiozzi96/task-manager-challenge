import { colors } from '../../../../../../core/theme/tokens';
import type { TaskPriority } from '../../../../domain/Task';

export const PRIORITY_COLOR: Record<TaskPriority, string> = {
  low:    colors.priority.low,
  medium: colors.priority.medium,
  high:   colors.priority.high,
  urgent: colors.priority.urgent,
};
