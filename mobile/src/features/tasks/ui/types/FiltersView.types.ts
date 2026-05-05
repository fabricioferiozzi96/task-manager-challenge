export interface FiltersViewProps {
  selectedStatusId: number | undefined;
  selectedPriorityId: number | undefined;
  onToggleStatus: (id: number | undefined) => void;
  onTogglePriority: (id: number | undefined) => void;
  onClear: () => void;
  onApply: () => void;
}
