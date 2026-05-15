import { StyleSheet } from 'react-native';
import { colors, radius, spacing, typography } from '../../../../core/theme/tokens';

export const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: spacing.xl,
    gap: spacing.md,
  },
  title: {
    ...typography.h2,
    textAlign: 'center',
  },
  loadingTitle: {
    ...typography.bodyMuted,
  },
  message: {
    ...typography.bodyMuted,
    textAlign: 'center',
  },
  button: {
    marginTop: spacing.md,
    paddingVertical: spacing.md,
    paddingHorizontal: spacing.xl,
    backgroundColor: colors.primary,
    borderRadius: radius.md,
  },
  buttonPressed: {
    opacity: 0.85,
  },
  buttonText: {
    color: colors.textInverse,
    fontWeight: '600',
    fontSize: 15,
  },
});
