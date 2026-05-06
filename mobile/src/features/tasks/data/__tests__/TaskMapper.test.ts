import { toTask } from '../TaskMapper';
import type { TaskDto } from '../TaskDto';

describe('TaskMapper.toTask', () => {
  const baseDto: TaskDto = {
    id: 1,
    title: 'Sample',
    description: 'A description',
    status: 'pending',
    statusLabel: 'Pendiente',
    priority: 'high',
    priorityLabel: 'Alta',
  };

  it('preserves status, priority and labels verbatim', () => {
    const task = toTask(baseDto);
    expect(task.status).toBe('pending');
    expect(task.statusLabel).toBe('Pendiente');
    expect(task.priority).toBe('high');
    expect(task.priorityLabel).toBe('Alta');
  });

  it('preserves nullable description', () => {
    const task = toTask({ ...baseDto, description: null });
    expect(task.description).toBeNull();
  });
});
