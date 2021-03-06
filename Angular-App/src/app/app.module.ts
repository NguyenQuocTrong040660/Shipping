import { SharedModule } from 'app/shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProgressBarModule } from 'primeng/progressbar';
import { AuthenticationService } from './shared/services/authentication.service';
import { HttpConfigInterceptor } from './shared/interceptors/http-config.interceptor';
import { LoadingService } from './shared/services/loading.service';
import { QuicklinkModule, QuicklinkStrategy } from 'ngx-quicklink';
import { appRoutes } from './routes';
import { NotificationService } from './shared/services/notification.service';
import { appInitializer } from 'app-initializer';
import { UnAuthorizedInterceptor } from './shared/interceptors/unauthorize.interceptor';
import { AuthorizeInterceptor } from './shared/interceptors/authorize.interceptor';
import { ApiClientModule } from './shared/api-clients/api-client.module';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ImportService } from './shared/services/import.service';
import { CommonService } from './shared/services/common.service';

const CUSTOM_SERVICES = [LoadingService, NotificationService, AuthenticationService, MessageService, ConfirmationService, DialogService, ImportService, CommonService];

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
    DynamicDialogModule,
    ApiClientModule,
    RouterModule.forRoot(appRoutes, { preloadingStrategy: QuicklinkStrategy }),
  ],
  providers: [
    ...CUSTOM_SERVICES,
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializer,
      multi: true,
      deps: [AuthenticationService],
    },
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UnAuthorizedInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
