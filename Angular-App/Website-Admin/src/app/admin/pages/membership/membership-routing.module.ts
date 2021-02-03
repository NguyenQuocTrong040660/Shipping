import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MemberShipComponent } from './container/membership.component';

const routes: Routes = [
  {
    path: '',
    component: MemberShipComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MemberShipRoutingModule { }
