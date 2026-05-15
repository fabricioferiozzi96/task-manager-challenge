import { StyleSheet } from 'react-native';
import { colors, spacing, typography } from '../../../../../../core/theme/tokens';

export const styles = StyleSheet.create({
  screen: {
    flex: 1,
    backgroundColor: colors.background,
  },
  content: {
    padding: spacing.xl,
    gap: spacing.lg,
  },
  title: {
    ...typography.h1,
  },
  badges: {
    flexDirection: 'row',
    gap: spacing.sm,
  },
  section: {
    gap: spacing.xs,
  },
  sectionLabel: {
    ...typography.small,
    textTransform: 'uppercase',
  },
  description: {
    ...typography.body,
    lineHeight: 22,
  },
});
