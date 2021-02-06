import { Routes } from '@angular/router';
import { NotFoundComponent } from './admin/pages/not-found/not-found.component';
import { AuthenticationGuard } from './shared/guards/authentication.guard';

export const appRoutes: Routes = [
  {
    path: '',
    canActivate: [AuthenticationGuard],
    loadChildren: () => import('./admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: 'login',
    loadChildren: () => import('./login/login.module').then((m) => m.LoginModule),
  },
  {
    path: 'forget-password',
    loadChildren: () => import('./forget-password/forget-password.module').then((m) => m.ForgetPasswordModule),
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];