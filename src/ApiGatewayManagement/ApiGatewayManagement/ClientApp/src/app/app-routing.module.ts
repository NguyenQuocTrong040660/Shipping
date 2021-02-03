import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './admin/pages/not-found/not-found.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./admin/admin.module').then((m) => m.AdminModule),
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadChildren: () => import('./admin/pages/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
