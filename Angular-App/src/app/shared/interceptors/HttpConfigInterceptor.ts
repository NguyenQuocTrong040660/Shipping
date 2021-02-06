import { Injectable, Injector } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { LoadingService } from '../services/loading.service';
import { finalize } from 'rxjs/operators';
import { StateService, States } from '../services/state.service';

@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {
  constructor(private loadingService: LoadingService, private injector: Injector) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loadingService.showLoading();
    if (request.url.startsWith('/api') && environment.production) {
      const baseUrl = environment.baseUrl;

      request = request.clone({
        url: baseUrl + request.url,
      });
    }

    const stateService = this.injector.get<StateService>(StateService);
    const accessToken = stateService.select(States.accessToken);
    if (!!accessToken) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        },
      });
    }
    return next.handle(request).pipe(finalize(() => this.loadingService.hideLoading()));
  }
}
