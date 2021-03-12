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
      // Shipping
      {
        path: 'shipping-request',
        loadChildren: () => import('./shipping/shipping-request/shipping-request.module').then((m) => m.ShippingRequestModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'shipping-mark',
        loadChildren: () => import('./shipping/shipping-mark/shipping-mark.module').then((m) => m.ShippingMarkModule),
        canActivate: [AuthenticationGuard],
      },
      // Movement
      {
        path: 'movement-request',
        loadChildren: () => import('./movement/movement-request/movement-request.module').then((m) => m.MovementRequestModule),
        canActivate: [AuthenticationGuard],
      },
      {
        path: 'received-mark',
        loadChildren: () => import('./movement/received-mark/received-mark.module').then((m) => m.ReceivedMarkModule),
        canActivate: [AuthenticationGuard],
      },
      // Management
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
      // Setting
      {
        path: 'user-management',
        loadChildren: () => import('./settings/user-management/user-management.module').then((m) => m.UserManagementModule),
        canActivate: [AuthenticationGuard, RoleGuard],
        data: { roles: [UserRole.SystemAdministrator, UserRole.ITAdministrator] },
      },
      {
        path: 'configuration',
        loadChildren: () => import('./settings/configuration/configuration.module').then((m) => m.ConfigurationModule),
        canActivate: [AuthenticationGuard, RoleGuard],
        data: { roles: [UserRole.SystemAdministrator, UserRole.ITAdministrator] },
      },
      {
        path: 'user-profile',
        loadChildren: () => import('./pages/user-profile/user-profile.module').then((m) => m.UserProfileModule),
        canActivate: [AuthenticationGuard],
      },
    ],
  },
];
