import React from 'react';
import { ScrollView, Text, View } from 'react-native';
import type { TaskDetailViewProps } from '../types/TaskDetailView.types';
import { styles } from '../styles/TaskDetailView.styles';
import { StateMessage } from '../../../../shared/components/StateMessage';
import { StatusBadge } from './StatusBadge';
import { PriorityBadge } from './PriorityBadge';

export const TaskDetailView: React.FC<TaskDetailViewProps> = ({
  task,
  isLoading,
  isError,
  onRetry,
}) => {
  if (isLoading) return <StateMessage variant="loading" />;
  if (isError || !task) return <StateMessage variant="error" onRetry={onRetry} />;

  return (
    <ScrollView style={styles.screen} contentContainerStyle={styles.content}>
      <Text style={styles.title}>{task.title}</Text>

      <View style={styles.badges}>
        <StatusBadge status={task.status} label={task.statusLabel} />
        <PriorityBadge priority={task.priority} label={task.priorityLabel} variant="solid" />
      </View>

      {task.description ? (
        <View style={styles.section}>
          <Text style={styles.sectionLabel}>Descripción</Text>
          <Text style={styles.description}>{task.description}</Text>
        </View>
      ) : null}
    </ScrollView>
  );
};
