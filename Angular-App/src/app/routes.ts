import { Routes } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
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
    path: '**',
    component: NotFoundComponent,
  },
];
