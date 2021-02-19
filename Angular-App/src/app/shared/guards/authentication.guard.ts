import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { StateService } from '../services/state.service';
import { UserClient } from 'app/shared/api-clients/user.client';
import { environment } from '../../../environments/environment';
import { NotificationService } from '../services/notification.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuard implements CanActivate {
  constructor(
    private router: Router,
    private stateService: StateService,
    private userClient: UserClient,
    private notificationService: NotificationService
  ) {}

  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (environment.production) {
      const accessToken = this.stateService.select('accessToken');
      if (accessToken) {
        this.userClient.apiUserUserInfo().subscribe((_) => {
          // this.notificationService.success('Login successfully');
          return true;
        });
      } else {
        this.router.navigateByUrl('/login');
        return false;
      }
    }

    return true;
  }
}
