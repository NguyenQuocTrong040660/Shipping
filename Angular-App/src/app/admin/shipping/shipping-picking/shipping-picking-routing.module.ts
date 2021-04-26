import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PrintComponent } from 'app/shared/components/print/print.component';
import { ShippingPickingComponent } from './shipping-picking.component';

const routes: Routes = [
  {
    path: '',
    component: ShippingPickingComponent,
  },
  {
    path: 'print',
    outlet: 'print',
    component: PrintComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ShippingPickingRoutingModule {}
