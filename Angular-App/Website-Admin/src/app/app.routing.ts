import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuicklinkStrategy, QuicklinkModule } from 'ngx-quicklink';
import { NotFoundComponent } from './admin/pages/not-found/not-found.component';
import { AuthorizeService } from './core/services/authorize.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpConfigInterceptor } from './core/interceptors/HttpConfigInterceptor';
import { environment } from 'environments/environment';
import { AlbumClient, API_BASE_URL } from './api-clients/album-client';
import { AlbumService } from './core/services/album.service';
import { ShippingAppClients } from './api-clients/shippingapp-client';
import { UserClient } from './api-clients/user-client';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];

@NgModule({
  imports: [QuicklinkModule, RouterModule.forRoot(routes, { preloadingStrategy: QuicklinkStrategy })],
  exports: [],
  providers: [
    AuthorizeService,
    AlbumClient,
    UserClient,
    AlbumService,
    { provide: API_BASE_URL, useValue: environment.baseUrl },
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true },
    ShippingAppClients,
  ],
})
export class AppRoutingModule { }
