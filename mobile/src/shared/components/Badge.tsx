import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { colors, radius, spacing, typography } from '../../core/theme/tokens';

interface BadgeProps {
  label: string;
  color: string;
  variant?: 'solid' | 'soft';
}

export const Badge: React.FC<BadgeProps> = ({ label, color, variant = 'soft' }) => {
  const isSolid = variant === 'solid';
  return (
    <View
      style={[
        styles.badge,
        {
          backgroundColor: isSolid ? color : color + '22',
          borderColor: color,
        },
      ]}
    >
      <Text
        style={[
          styles.text,
          { color: isSolid ? colors.textInverse : color },
        ]}
        numberOfLines={1}
      >
        {label.toUpperCase()}
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
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
