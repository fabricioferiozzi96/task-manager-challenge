import React from 'react';
import { Text, View } from 'react-native';
import { colors } from '../../../core/theme/tokens';
import { styles } from './styles/Badge.styles';
import type { BadgeProps } from './types/Badge.types';

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
