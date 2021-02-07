import { AuthenticationService } from 'app/shared/services/authentication.service';

export function appInitializer(authService: AuthenticationService) {
  return () =>
    new Promise((resolve) => {
      console.log('check connect from client and server');
      authService.initUserLoggedIn().add(resolve);
    });
}
