import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { LoadingService } from '../services/loading.service';
import { finalize } from 'rxjs/operators';

@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {
  constructor(private loadingService: LoadingService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loadingService.showLoading();
    if (request.url.startsWith('/api') && environment.production) {
      const baseUrl = environment.baseUrl;

      request = request.clone({
        url: baseUrl + request.url,
      });
    }

    return next.handle(request).pipe(finalize(() => this.loadingService.hideLoading()));
  }
}
