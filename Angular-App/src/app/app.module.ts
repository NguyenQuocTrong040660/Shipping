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
import { AlbumClient, API_BASE_URL } from './shared/api-clients/album-client';
import { UserClient } from './shared/api-clients/user-client';
import { AlbumService } from './shared/services/album.service';
import { ShippingAppClients } from './shared/api-clients/shippingapp-client';
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
    AlbumClient,
    UserClient,
    AlbumService,
    ShippingAppClients,
    { provide: API_BASE_URL, useValue: environment.baseUrl },
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }],
  bootstrap: [AppComponent],
})
export class AppModule {}
