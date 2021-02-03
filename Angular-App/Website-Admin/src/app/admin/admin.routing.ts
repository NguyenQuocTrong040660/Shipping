import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizeGuard } from 'app/core/guards/authorize.guard';
import { AdminComponent } from './container/admin.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: 'files-management',
        loadChildren: () =>
          import('./pages/files-management/files-management.module').then((m) => m.FilesManagementModule),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'attachment-types-management',
        loadChildren: () =>
          import('./pages/attachment-types-management/attachment-types-management.module').then(
            (m) => m.AttachmentTypesManagementModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'youtube-embed-management',
        loadChildren: () =>
          import('./pages/youtube-embed-managements/youtube-embed-managements.module').then(
            (m) => m.YoutubeEmbedManagementsModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'product',
        loadChildren: () =>
          import('./pages/product/product.module').then(
            (m) => m.ProductModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'product-type',
        loadChildren: () =>
          import('./pages/product-type/product-type.module').then(
            (m) => m.ProductTypeModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'brand',
        loadChildren: () =>
          import('./pages/brand/brand.module').then(
            (m) => m.BrandModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'country',
        loadChildren: () =>
          import('./pages/country/country.module').then(
            (m) => m.CountryModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'reservation',
        loadChildren: () =>
          import('./pages/reservation/reservation.module').then(
            (m) => m.ReservationModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'membership',
        loadChildren: () =>
          import('./pages/membership/membership.module').then(
            (m) => m.MemberShipModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: 'promotion',
        loadChildren: () =>
          import('./pages/promotion/promotion.module').then(
            (m) => m.PromotionModule
          ),
        // canActivate: [AuthorizeGuard],
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: '/',
      },
    ],
  },
  {
    path: 'login',
    loadChildren: () => import('./pages/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: 'register',
    loadChildren: () => import('./pages/register/register.module').then((m) => m.RegisterModule),
  },
  {
    path: 'forget-password',
    loadChildren: () => import('./pages/forget-password/forget-password.module').then((m) => m.ForgetPasswordModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule { }
