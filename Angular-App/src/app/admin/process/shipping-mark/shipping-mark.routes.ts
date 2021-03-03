import { Routes } from '@angular/router';
import { ShippingMarkComponent } from './shipping-mark.component';
import { PrintComponent } from '../../../shared/components/print/print.component';

export const shippingMarkRoutes: Routes = [
  {
    path: '',
    component: ShippingMarkComponent,
  },
  {
    path: 'print',
    outlet: 'print',
    component: PrintComponent,
  },
];
