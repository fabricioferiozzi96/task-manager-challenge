/**
 * Design tokens. Toda la app consume desde aquí — no hardcodear colores ni medidas.
 */

export const colors = {
  background: '#F5F7FA',
  surface: '#FFFFFF',
  surfaceMuted: '#F1F5F9',
  border: '#E2E8F0',

  text: '#0F172A',
  textMuted: '#64748B',
  textInverse: '#FFFFFF',

  primary: '#3B82F6',
  primaryMuted: '#DBEAFE',

  // Estados
  status: {
    pending: '#F59E0B',
    inProgress: '#3B82F6',
    completed: '#10B981',
    cancelled: '#94A3B8',
  },

  // Prioridades
  priority: {
    low: '#94A3B8',
    medium: '#3B82F6',
    high: '#F59E0B',
    urgent: '#EF4444',
  },

  danger: '#EF4444',
} as const;

export const spacing = {
  xs: 4,
  sm: 8,
  md: 12,
  lg: 16,
  xl: 24,
  xxl: 32,
} as const;

export const radius = {
  sm: 6,
  md: 10,
  lg: 16,
  pill: 999,
} as const;

export const typography = {
  h1: { fontSize: 26, fontWeight: '700' as const, color: colors.text },
  h2: { fontSize: 20, fontWeight: '700' as const, color: colors.text },
  h3: { fontSize: 17, fontWeight: '600' as const, color: colors.text },
  body: { fontSize: 15, fontWeight: '400' as const, color: colors.text },
  bodyMuted: { fontSize: 14, fontWeight: '400' as const, color: colors.textMuted },
  small: { fontSize: 12, fontWeight: '500' as const, color: colors.textMuted },
  badge: { fontSize: 11, fontWeight: '700' as const, letterSpacing: 0.5 },
} as const;
