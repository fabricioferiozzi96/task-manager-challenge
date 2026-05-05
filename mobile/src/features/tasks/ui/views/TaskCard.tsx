import React, { memo } from 'react';
import { Pressable, Text, View } from 'react-native';
import type { TaskCardProps } from '../types/TaskCard.types';
import { styles } from '../styles/TaskCard.styles';
import { StatusBadge } from './StatusBadge';
import { PriorityBadge } from './PriorityBadge';

const TaskCardComponent: React.FC<TaskCardProps> = ({ task, onPress }) => {
  return (
    <Pressable
      onPress={() => onPress(task.id)}
      style={({ pressed }) => [styles.card, pressed && styles.pressed]}
      accessibilityRole="button"
    >
      <View style={styles.header}>
        <Text style={styles.title} numberOfLines={2}>
          {task.title}
        </Text>
      </View>

      {task.description ? (
        <Text style={styles.description} numberOfLines={2}>
          {task.description}
        </Text>
      ) : null}

      <View style={styles.footer}>
        <View style={styles.badges}>
          <StatusBadge status={task.status} label={task.statusLabel} />
          <PriorityBadge priority={task.priority} label={task.priorityLabel} />
        </View>
      </View>
    </Pressable>
  );
};

export const TaskCard = memo(TaskCardComponent);
