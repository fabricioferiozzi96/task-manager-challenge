import { Pressable, ScrollView, Text, View } from 'react-native';
import { styles } from './styles/FiltersView.styles';
import type { FiltersViewProps } from './types/FiltersView.types';
import { PRIORITY_OPTIONS, STATUS_OPTIONS } from '../../../domain/Task';

export const FiltersView = ({
  selectedStatusId,
  selectedPriorityId,
  onToggleStatus,
  onTogglePriority,
  onClear,
  onApply,
}: FiltersViewProps) => (
  <ScrollView style={styles.screen} contentContainerStyle={styles.content}>
    <View style={styles.section}>
      <Text style={styles.sectionTitle}>Estado</Text>
      <View style={styles.options}>
        {STATUS_OPTIONS.map((opt) => {
          const active = selectedStatusId === opt.id;
          return (
            <ChipOption
              key={opt.id}
              label={opt.label}
              active={active}
              onPress={() => onToggleStatus(active ? undefined : opt.id)}
            />
          );
        })}
      </View>
    </View>

    <View style={styles.section}>
      <Text style={styles.sectionTitle}>Prioridad</Text>
      <View style={styles.options}>
        {PRIORITY_OPTIONS.map((opt) => {
          const active = selectedPriorityId === opt.id;
          return (
            <ChipOption
              key={opt.id}
              label={opt.label}
              active={active}
              onPress={() => onTogglePriority(active ? undefined : opt.id)}
            />
          );
        })}
      </View>
    </View>

    <View style={styles.actions}>
      <Pressable
        onPress={onClear}
        style={({ pressed }) => [styles.secondaryBtn, pressed && styles.pressed]}
      >
        <Text style={styles.secondaryBtnText}>Limpiar</Text>
      </Pressable>
      <Pressable
        onPress={onApply}
        style={({ pressed }) => [styles.primaryBtn, pressed && styles.pressed]}
      >
        <Text style={styles.primaryBtnText}>Aplicar</Text>
      </Pressable>
    </View>
  </ScrollView>
);

interface ChipOptionProps {
  label: string;
  active: boolean;
  onPress: () => void;
}

const ChipOption = ({ label, active, onPress }: ChipOptionProps) => (
  <Pressable
    onPress={onPress}
    style={({ pressed }) => [
      styles.chip,
      active && styles.chipActive,
      pressed && styles.pressed,
    ]}
    accessibilityRole="button"
    accessibilityState={{ selected: active }}
  >
    <Text style={[styles.chipText, active && styles.chipTextActive]}>{label}</Text>
  </Pressable>
);
