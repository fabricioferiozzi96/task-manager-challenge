import { useCallback } from 'react';
import { FlatList, Pressable, Text, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { styles } from './styles/TaskListView.styles';
import type { TaskListViewProps } from './types/TaskListView.types';
import { StateMessage } from '../../../../../shared/components/StateMessage';
import { TaskCard } from '../TaskCard';
import type { Task } from '../../../domain/Task';

export const TaskListView = ({
  tasks,
  isLoading,
  isError,
  isFetching,
  activeFilterLabels,
  onTaskPress,
  onFilterPress,
  onClearFilters,
  onRetry,
  onRefresh,
}: TaskListViewProps) => {
  const renderItem = useCallback(
    ({ item }: { item: Task }) => <TaskCard task={item} onPress={onTaskPress} />,
    [onTaskPress],
  );

  const hasActiveFilters = activeFilterLabels.length > 0;
  const count = tasks?.length ?? 0;

  return (
    <SafeAreaView style={styles.screen} edges={['top']}>
      <View style={styles.header}>
        <View>
          <Text style={styles.headerTitle}>Mis tareas</Text>
          <Text style={styles.headerSubtitle}>
            {tasks ? `${count} ${count === 1 ? 'tarea' : 'tareas'}` : ''}
          </Text>
        </View>
        <Pressable
          onPress={onFilterPress}
          style={({ pressed }) => [styles.filterButton, pressed && styles.pressed]}
          accessibilityLabel="Filtrar tareas"
          hitSlop={8}
        >
          <Text style={styles.filterButtonText}>
            Filtrar{hasActiveFilters ? ` · ${activeFilterLabels.length}` : ''}
          </Text>
        </Pressable>
      </View>

      {hasActiveFilters ? (
        <View style={styles.activeFilters}>
          {activeFilterLabels.map((chip) => (
            <View key={chip} style={styles.chip}>
              <Text style={styles.chipText}>{chip}</Text>
            </View>
          ))}
          <Pressable onPress={onClearFilters}>
            <Text style={styles.clearText}>Limpiar</Text>
          </Pressable>
        </View>
      ) : null}

      {isLoading ? (
        <StateMessage variant="loading" />
      ) : isError ? (
        <StateMessage variant="error" onRetry={onRetry} />
      ) : !tasks || tasks.length === 0 ? (
        <StateMessage variant="empty" />
      ) : (
        <FlatList
          data={tasks}
          renderItem={renderItem}
          keyExtractor={(t) => String(t.id)}
          contentContainerStyle={styles.list}
          showsVerticalScrollIndicator={false}
          refreshing={isFetching && !isLoading}
          onRefresh={onRefresh}
        />
      )}
    </SafeAreaView>
  );
};
