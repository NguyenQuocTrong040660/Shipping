import { Routes } from '@angular/router';
import { UserRole } from 'app/shared/constants/user-role.constants';
import { AuthenticationGuard } from 'app/shared/guards/authentication.guard';
import { RoleGuard } from 'app/shared/guards/role.guard';
import { AdminComponent } from './admin.component';

export const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthenticationGuard],
    children: [
      // Process
      {
        path: 'shipping-request',
        loadChildren: () => import('./process/shipping-request/shipping-request.module').then((m) => m.ShippingRequestModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'movement-request',
        loadChildren: () => import('./process/movement-request/movement-request.module').then((m) => m.MovementRequestModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'received-mark',
        loadChildren: () => import('./process/received-mark/received-mark.module').then((m) => m.ReceivedMarkModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'shipping-mark',
        loadChildren: () => import('./process/shipping-mark/shipping-mark.module').then((m) => m.ShippingMarkModule),
        canActivate: [AuthenticationGuard],
      },
      // Management
      {
        path: 'user-management',
        loadChildren: () => import('./management/user-management/user-management.module').then((m) => m.UserManagementModule),
        canActivate: [AuthenticationGuard, RoleGuard],
        data: { roles: [UserRole.SystemAdministrator, UserRole.ITAdministrator] },
      },
      {
        path: 'product',
        loadChildren: () => import('./management/product/product.module').then((m) => m.ProductModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'work-order',
        loadChildren: () => import('./management/work-order/work-order.module').then((m) => m.WorkOrderModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'shipping-plan',
        loadChildren: () => import('./management/shipping-plan/shipping-plan.module').then((m) => m.ShippingPlanModule),
        canActivate: [AuthenticationGuard],
      },
      // Settings
      {
        path: 'configuration',
        loadChildren: () => import('./settings/configuration/configuration.module').then((m) => m.ConfigurationModule),
        canActivate: [AuthenticationGuard, RoleGuard],
        data: { roles: [UserRole.SystemAdministrator, UserRole.ITAdministrator] },
      },
      {
        path: 'files-management',
        loadChildren: () => import('./pages/files-management/files-management.module').then((m) => m.FilesManagementModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'attachment-types-management',
        loadChildren: () => import('./pages/attachment-types-management/attachment-types-management.module').then((m) => m.AttachmentTypesManagementModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'files-management',
        loadChildren: () => import('./pages/files-management/files-management.module').then((m) => m.FilesManagementModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'attachment-types-management',
        loadChildren: () => import('./pages/attachment-types-management/attachment-types-management.module').then((m) => m.AttachmentTypesManagementModule),
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
    ],
  },
];
