import { Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'environments/environment';
import { BehaviorSubject, Observable, of, Subscription, throwError } from 'rxjs';
import { catchError, delay, finalize, tap } from 'rxjs/operators';
import { LoginRequest, IdentityResult, RefreshTokenRequest, UserClient } from '../api-clients/user.client';
import { Roles } from '../enumerations/roles.enum';
import { ApplicationUser } from '../models/application-user';
import { NotificationService } from './notification.service';
import { States, StateService } from './state.service';
@Injectable({
  providedIn: 'root',
})
export class AuthenticationService implements OnDestroy {
  private timer: Subscription;
  private _user = new BehaviorSubject<ApplicationUser>(null);
  user$: Observable<ApplicationUser> = this._user.asObservable();

  constructor(private router: Router, private usersClient: UserClient, private stateService: StateService, private notificationService: NotificationService) {
    console.log('init AuthenticationService');
    window.addEventListener('storage', this.storageEventListener.bind(this));
  }

  handleError(error) {
    const errorMessage = error.error instanceof ErrorEvent ? `Error: ${error.error.message}` : `Error Code: ${error.status}\n Message: ${error.message}`;
    return throwError(errorMessage);
  }

  login(userName: string, password: string, rememberMe: boolean) {
    const loginRequest: LoginRequest = {
      userName,
      password,
      rememberMe,
    };

    if (!environment.production) {
      const mockIdentityResult: IdentityResult = {
        succeeded: true,
        userName: 'admin',
        roles: [Roles.SystemAdministrator],
        originalUserName: 'admin',
      };

      this._user.next({
        userName: mockIdentityResult.userName,
        roles: mockIdentityResult.roles,
        originalUserName: mockIdentityResult.originalUserName,
      });
      return of(mockIdentityResult);
    }

    return this.usersClient.apiUserUserLogin(loginRequest).pipe(
      tap((data: IdentityResult) => {
        if (data && data.succeeded) {
          this._user.next({
            userName: data.userName,
            roles: data.roles,
            originalUserName: data.originalUserName,
          });
          this.setLocalStorage(data);
          this.startTokenTimer();
        }

        return data;
      }),
      catchError((err) => this.handleError(err))
    );
  }

  logout() {
    this.usersClient
      .apiUserUserLogout()
      .pipe(
        finalize(() => {
          this.clearLocalStorage();
          this._user.next(null);
          this.stopTokenTimer();
          this.router.navigate(['login']);
        })
      )
      .subscribe();
  }

  initUserLoggedIn() {
    return this.usersClient.apiUserUserInfo().subscribe(
      (x: IdentityResult) =>
        this._user.next({
          userName: x.userName,
          roles: x.roles,
          originalUserName: x.originalUserName,
        }),
      (_) => {
        console.log('User logged out');
        this.clearLocalStorage();
        this.router.navigate(['login']);
      }
    );
  }

  refreshToken() {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      this.clearLocalStorage();
      return of(null);
    }

    const request: RefreshTokenRequest = {
      refreshToken: refreshToken,
    };

    return this.usersClient.apiUserUserRefreshToken(request).pipe(
      tap((data: IdentityResult) => {
        this._user.next({
          userName: data.userName,
          roles: data.roles,
          originalUserName: data.originalUserName,
        });
        this.setLocalStorage(data);
        this.startTokenTimer();
        return data;
      }),
      catchError((err) => this.handleError(err))
    );
  }

  setLocalStorage(result: IdentityResult) {
    this.stateService.setState(States.AccessToken, result.accessToken);
    this.stateService.setState(States.RefreshToken, result.refreshToken);
    this.stateService.setState(States.LoginEvent, 'login' + Math.random());
  }

  getAccessToken() {
    return this.stateService.select(States.AccessToken);
  }

  getRefreshToken() {
    return this.stateService.select(States.RefreshToken);
  }

  clearLocalStorage() {
    this.stateService.resetState(States.AccessToken);
    this.stateService.resetState(States.RefreshToken);
    this.stateService.setState(States.LoginEvent, 'login' + Math.random());
  }

  private getTokenRemainingTime() {
    const accessToken = this.getAccessToken();
    if (!accessToken) {
      return 0;
    }

    const jwtToken = JSON.parse(atob(accessToken.split('.')[1]));
    const expires = new Date(jwtToken.exp * 1000);
    return expires.getTime() - Date.now();
  }

  private startTokenTimer() {
    const timeout = this.getTokenRemainingTime();
    this.timer = of(true)
      .pipe(
        delay(timeout),
        tap(() => this.refreshToken().subscribe())
      )
      .subscribe();
  }

  private storageEventListener(event: StorageEvent) {
    if (event.storageArea === localStorage) {
      if (event.key === States.LoginEvent) {
        this.stopTokenTimer();
        this._user.next(null);
      }

      if (event.key === States.LoginEvent) {
        this.stopTokenTimer();
        this.usersClient.apiUserUserInfo().subscribe((x: IdentityResult) =>
          this._user.next({
            userName: x.userName,
            roles: x.roles,
            originalUserName: x.originalUserName,
          })
        );
      }
    }
  }

  private stopTokenTimer() {
    this.timer?.unsubscribe();
  }

  ngOnDestroy(): void {
    window.removeEventListener('storage', this.storageEventListener.bind(this));
  }
}
