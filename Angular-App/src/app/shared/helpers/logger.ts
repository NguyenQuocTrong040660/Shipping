import { environment } from 'environments/environment';

export default class Logger {
  public static LogInfo(message: any) {
    if (!environment.production) {
      console.log(message);
    }
  }

  public static LogError(error: any) {
    if (!environment.production) {
      console.error(error);
    }
  }

  public static LogWarning(message: any) {
    if (!environment.production) {
      console.warn(message);
    }
  }
}
