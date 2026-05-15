import React from 'react';
import { fireEvent, render, screen } from '@testing-library/react-native';
import { TaskCard } from '../TaskCard';
import type { Task } from '../../../../domain/Task';

const buildTask = (overrides: Partial<Task> = {}): Task => ({
  id: 1,
  title: 'Refactor auth module',
  description: 'Clean up the legacy session middleware',
  status: 'in_progress',
  statusLabel: 'En progreso',
  priority: 'high',
  priorityLabel: 'Alta',
  ...overrides,
});

describe('<TaskCard />', () => {
  it('renders title, description, and badge labels', () => {
    render(<TaskCard task={buildTask()} onPress={jest.fn()} />);

    expect(screen.getByText('Refactor auth module')).toBeOnTheScreen();
    expect(screen.getByText('Clean up the legacy session middleware')).toBeOnTheScreen();
    expect(screen.getByText('EN PROGRESO')).toBeOnTheScreen();
    expect(screen.getByText('ALTA')).toBeOnTheScreen();
  });

  it('omits the description block when description is null', () => {
    render(<TaskCard task={buildTask({ description: null })} onPress={jest.fn()} />);
    expect(
      screen.queryByText('Clean up the legacy session middleware'),
    ).toBeNull();
  });

  it('invokes onPress with the task id when tapped', () => {
    const onPress = jest.fn();
    render(<TaskCard task={buildTask({ id: 42 })} onPress={onPress} />);

    fireEvent.press(screen.getByRole('button'));

    expect(onPress).toHaveBeenCalledWith(42);
  });
});
