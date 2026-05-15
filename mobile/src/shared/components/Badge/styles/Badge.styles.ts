import { StyleSheet } from 'react-native';
import { radius, spacing, typography } from '../../../../core/theme/tokens';

export const styles = StyleSheet.create({
  badge: {
    paddingHorizontal: spacing.sm,
    paddingVertical: 3,
    borderRadius: radius.pill,
    borderWidth: StyleSheet.hairlineWidth,
    alignSelf: 'flex-start',
  },
  text: {
    ...typography.badge,
  },
});
