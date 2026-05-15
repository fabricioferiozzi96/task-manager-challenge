import { colors } from '../../../../../../core/theme/tokens';
import type { TaskStatus } from '../../../../domain/Task';

export const STATUS_COLOR: Record<TaskStatus, string> = {
  pending:     colors.status.pending,
  in_progress: colors.status.inProgress,
  completed:   colors.status.completed,
  cancelled:   colors.status.cancelled,
};
