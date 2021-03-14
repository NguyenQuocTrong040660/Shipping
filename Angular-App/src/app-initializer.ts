import Logger from 'app/shared/helpers/logger';
import { AuthenticationService } from 'app/shared/services/authentication.service';

export function appInitializer(authService: AuthenticationService) {
  return () =>
    new Promise((resolve) => {
      Logger.LogInfo('check connect from client and server');
      authService.initUserLoggedIn().add(resolve);
    });
}
