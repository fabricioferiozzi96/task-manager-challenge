import type { Task } from '../domain/Task';
import type { TaskDto } from './TaskDto';

/**
 * Mapper DTO → entidad de dominio.
 *
 * Hoy es prácticamente identidad porque el DTO de la API ya tiene la forma
 * que consume la app. Lo mantenemos como capa de aislamiento: si el backend
 * cambia un nombre de campo, solo este archivo se toca.
 *
 * Las fechas se mantienen como ISO strings (no se convierten a Date) porque
 * el estado de Redux debe ser serializable. La conversión a `Date` ocurre en
 * la capa de presentación, al formatear para mostrar.
 */
export const toTask = (dto: TaskDto): Task => ({
  id: dto.id,
  title: dto.title,
  description: dto.description,
  status: dto.status,
  statusLabel: dto.statusLabel,
  priority: dto.priority,
  priorityLabel: dto.priorityLabel,
});
