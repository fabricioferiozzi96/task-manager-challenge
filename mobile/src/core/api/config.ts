import { Platform } from 'react-native';

/**
 * Base URL del backend.
 * En Android, el emulador alias `10.0.2.2` apunta al `localhost` del host.
 * En iOS Simulator, `localhost` funciona directo.
 */
const HOST = Platform.select({
  android: '10.0.2.2',
  ios: 'localhost',
  default: 'localhost',
});

export const API_BASE_URL = `http://${HOST}:5186/api`;
