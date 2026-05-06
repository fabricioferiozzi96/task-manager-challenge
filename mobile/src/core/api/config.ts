import { Platform } from 'react-native';
import { API_URL_ANDROID, API_URL_IOS } from '@env';

/**
 * URL base del backend. Las URLs vienen del archivo .env (ver .env.example).
 *
 * Android emulator: 10.0.2.2 (alias del emulador para el localhost del host).
 * iOS Simulator: localhost (comparte red con la Mac).
 * Device físico: usar la IP de la máquina en la red local.
 */
export const API_BASE_URL = Platform.select({
  android: API_URL_ANDROID,
  ios: API_URL_IOS,
  default: API_URL_IOS,
});
