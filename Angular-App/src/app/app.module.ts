import { SharedModule } from 'app/shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProgressBarModule } from 'primeng/progressbar';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AuthorizeService } from './shared/services/authorize.service';
import { API_BASE_URL, FilesClient } from './shared/api-clients/files.client';
import { UserClient } from './shared/api-clients/user.client';
import { ShippingAppClients } from './shared/api-clients/shipping-app.client';
import { environment } from 'environments/environment';
import { HttpConfigInterceptor } from './shared/interceptors/HttpConfigInterceptor';
import { LoadingService } from './shared/services/loading.service';
import { QuicklinkModule, QuicklinkStrategy } from 'ngx-quicklink';
import { appRoutes } from './routes';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserAnimationsModule,
    CommonModule,
    BrowserModule,
    HttpClientModule,
    ProgressBarModule,
    SharedModule,
    QuicklinkModule,
    RouterModule.forRoot(appRoutes, { preloadingStrategy: QuicklinkStrategy }),
  ],
  providers: [
    LoadingService,
    MessageService,
    ConfirmationService,
    AuthorizeService,
    FilesClient,
    UserClient,
    ShippingAppClients,
    { provide: API_BASE_URL, useValue: environment.baseUrl },
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
