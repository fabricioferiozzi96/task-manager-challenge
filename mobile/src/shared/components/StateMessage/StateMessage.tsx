import React from 'react';
import { ActivityIndicator, Pressable, Text, View } from 'react-native';
import { colors } from '../../../core/theme/tokens';
import { styles } from './styles/StateMessage.styles';
import type { StateMessageProps } from './types/StateMessage.types';

const DEFAULTS = {
  loading: { title: 'Cargando…', message: '' },
  empty:   { title: 'Nada por aquí', message: 'No hay tareas que coincidan con los filtros.' },
  error:   { title: 'Algo salió mal', message: 'No pudimos cargar la información. Intentá de nuevo.' },
};

export const StateMessage: React.FC<StateMessageProps> = ({ variant, title, message, onRetry }) => {
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
