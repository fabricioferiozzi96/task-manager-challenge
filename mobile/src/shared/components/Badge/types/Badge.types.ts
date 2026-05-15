export type BadgeVariant = 'solid' | 'soft';

export interface BadgeProps {
  label: string;
  color: string;
  variant?: BadgeVariant;
}
