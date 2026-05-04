import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import type { TaskFilters } from '../domain/Task';

const initialState: TaskFilters = {
  status: undefined,
  priority: undefined,
};

/**
 * Filtros activos. Client-state, separado del server-state (que está en RTK Query).
 * Esta separación evita el antipatrón de "cachear datos del server en Redux a mano".
 */
const filtersSlice = createSlice({
  name: 'filters',
  initialState,
  reducers: {
    setStatus(state, action: PayloadAction<number | undefined>) {
      state.status = action.payload;
    },
    setPriority(state, action: PayloadAction<number | undefined>) {
      state.priority = action.payload;
    },
    clearFilters() {
      return initialState;
    },
  },
});

export const { setStatus, setPriority, clearFilters } = filtersSlice.actions;
export const filtersReducer = filtersSlice.reducer;
