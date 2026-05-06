import {
  clearFilters,
  filtersReducer,
  setPriority,
  setStatus,
} from '../filtersSlice';

describe('filtersSlice', () => {
  const initial = { status: undefined, priority: undefined };

  it('starts with no filters active', () => {
    expect(filtersReducer(undefined, { type: 'init' })).toEqual(initial);
  });

  it('setStatus updates only the status field', () => {
    const next = filtersReducer(initial, setStatus(2));
    expect(next).toEqual({ status: 2, priority: undefined });
  });

  it('setPriority updates only the priority field', () => {
    const next = filtersReducer(initial, setPriority(4));
    expect(next).toEqual({ status: undefined, priority: 4 });
  });

  it('setStatus(undefined) clears the status filter', () => {
    const withStatus = filtersReducer(initial, setStatus(1));
    const cleared = filtersReducer(withStatus, setStatus(undefined));
    expect(cleared.status).toBeUndefined();
  });

  it('clearFilters resets to initial state', () => {
    const withBoth = filtersReducer(
      filtersReducer(initial, setStatus(2)),
      setPriority(3),
    );
    expect(withBoth).toEqual({ status: 2, priority: 3 });

    const cleared = filtersReducer(withBoth, clearFilters());
    expect(cleared).toEqual(initial);
  });
});
