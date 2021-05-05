import { Injectable, Injector } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { States, StateService } from '../services/state.service';

@Injectable()
export class AuthorizeInterceptor implements HttpInterceptor {
  constructor(private injector: Injector) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const stateService = this.injector.get<StateService>(StateService);
    const accessToken = stateService.select(States.AccessToken);
    const isApi = request.url.includes('/api/');

    if (accessToken && isApi) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        },
      });
    }

    return next.handle(request);
  }
}
