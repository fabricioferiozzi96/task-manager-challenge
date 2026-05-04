import React from 'react';
import { ActivityIndicator, Pressable, StyleSheet, Text, View } from 'react-native';
import { colors, radius, spacing, typography } from '../../core/theme/tokens';

interface Props {
  variant: 'loading' | 'empty' | 'error';
  title?: string;
  message?: string;
  onRetry?: () => void;
}

const DEFAULTS = {
  loading: { title: 'Cargando…', message: '' },
  empty:   { title: 'Nada por aquí', message: 'No hay tareas que coincidan con los filtros.' },
  error:   { title: 'Algo salió mal', message: 'No pudimos cargar la información. Intentá de nuevo.' },
};

export const StateMessage: React.FC<Props> = ({ variant, title, message, onRetry }) => {
  const t = title ?? DEFAULTS[variant].title;
  const m = message ?? DEFAULTS[variant].message;

  return (
    <View style={styles.container}>
      {variant === 'loading' && <ActivityIndicator size="large" color={colors.primary} />}
      <Text style={[styles.title, variant === 'loading' && styles.loadingTitle]}>{t}</Text>
      {m ? <Text style={styles.message}>{m}</Text> : null}
      {onRetry && variant === 'error' ? (
        <Pressable onPress={onRetry} style={({ pressed }) => [styles.button, pressed && styles.buttonPressed]}>
          <Text style={styles.buttonText}>Reintentar</Text>
        </Pressable>
      ) : null}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: spacing.xl,
    gap: spacing.md,
  },
  title: {
    ...typography.h2,
    textAlign: 'center',
  },
  loadingTitle: {
    ...typography.bodyMuted,
  },
  message: {
    ...typography.bodyMuted,
    textAlign: 'center',
  },
  button: {
    marginTop: spacing.md,
    paddingVertical: spacing.md,
    paddingHorizontal: spacing.xl,
    backgroundColor: colors.primary,
    borderRadius: radius.md,
  },
  buttonPressed: {
    opacity: 0.85,
  },
  buttonText: {
    color: colors.textInverse,
    fontWeight: '600',
    fontSize: 15,
  },
});
