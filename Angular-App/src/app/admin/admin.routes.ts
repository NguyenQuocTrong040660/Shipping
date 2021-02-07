import { Routes } from '@angular/router';
import { AuthenticationGuard } from 'app/shared/guards/authentication.guard';
import { AdminComponent } from './admin.component';

export const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthenticationGuard],
    children: [
      {
        path: 'files-management',
        loadChildren: () =>
          import('./pages/files-management/files-management.module').then((m) => m.FilesManagementModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'attachment-types-management',
        loadChildren: () =>
          import('./pages/attachment-types-management/attachment-types-management.module').then(
            (m) => m.AttachmentTypesManagementModule
          ),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'product',
        loadChildren: () => import('./pages/product/product.module').then((m) => m.ProductModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'product-type',
        loadChildren: () => import('./pages/product-type/product-type.module').then((m) => m.ProductTypeModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'country',
        loadChildren: () => import('./pages/country/country.module').then((m) => m.CountryModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: '/',
      },
      {
        path: 'user-profile',
        loadChildren: () => import('./pages/user-profile/user-profile.module').then((m) => m.UserProfileModule),
        canActivate: [AuthenticationGuard],
      },
    ],
  },
  {
    path: 'register',
    loadChildren: () => import('./pages/register/register.module').then((m) => m.RegisterModule),
  },
];
