export type StateMessageVariant = 'loading' | 'empty' | 'error';

export interface StateMessageProps {
  variant: StateMessageVariant;
  title?: string;
  message?: string;
  onRetry?: () => void;
}
