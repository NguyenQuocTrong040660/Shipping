import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import Logger from '../helpers/logger';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class UnAuthorizedInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err) => {
        if (err.status === 401) {
          this.tryRefreshToken();
        }

        Logger.LogError(err);

        const error = (err && err.error && err.error.message) || err.statusText;
        return throwError(error);
      })
    );
  }

  tryRefreshToken() {
    return this.authenticationService.refreshToken().subscribe(
      (_) => Logger.LogInfo('try to refresh token'),
      (_) => this.doWhenRefreshFailed()
    );
  }

  doWhenRefreshFailed() {
    Logger.LogInfo('clean when try failed');
    this.authenticationService.clearLocalStorage();
    this.router.navigate(['login']);
  }
}
