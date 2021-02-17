import { Routes } from '@angular/router';
import { PrintComponent } from 'app/shared/components/print/print.component';
import { ReceivedMarkComponent } from './received-mark.component';

export const receivedMarkRoutes: Routes = [
  {
    path: '',
    component: ReceivedMarkComponent
  },
  {
    path: 'print',
    outlet: 'print',
    component: PrintComponent
  }];
