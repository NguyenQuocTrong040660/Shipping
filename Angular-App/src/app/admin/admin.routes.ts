import { Routes } from '@angular/router';
import { AuthenticationGuard } from 'app/shared/guards/authentication.guard';
import { AdminComponent } from './admin.component';

export const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthenticationGuard],
    children: [
      // Business
      {
        path: 'shipping-plan',
        loadChildren: () =>
          import('./business/shipping-plan/shipping-plan.module').then((m) => m.ShippingPlanModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'shipping-request',
        loadChildren: () =>
          import('./business/shiping-request/shiping-request.module').then((m) => m.ShippingRequestModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'movement-request',
        loadChildren: () =>
          import('./business/movement-request/movement-request.module').then((m) => m.MovementRequestModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'received-mark',
        loadChildren: () =>
          import('./business/received-mark/received-mark.module').then((m) => m.ReceivedMarkModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'shipping-mark',
        loadChildren: () =>
          import('./business/shipping-mark/shipping-mark.module').then((m) => m.ShippingMarkModule),
        canActivate: [AuthenticationGuard],
      },
      // Management
      {
        path: 'user-management',
        loadChildren: () => import('./management/user-management/user-management.module').then((m) => m.UserManagementModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'product',
        loadChildren: () => import('./management/product/product.module').then((m) => m.ProductModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'product-type',
        loadChildren: () => import('./management/product-type/product-type.module').then((m) => m.ProductTypeModule),
        canActivate: [AuthenticationGuard],
      },
      // Settings
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
        path: 'country',
        loadChildren: () => import('./pages/country/country.module').then((m) => m.CountryModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'user-profile',
        loadChildren: () => import('./pages/user-profile/user-profile.module').then((m) => m.UserProfileModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: '/',
      }
    ],
  },
];
