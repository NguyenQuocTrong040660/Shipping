import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ShippingRequestComponent } from './shiping-request.component';
import { shippingRequestRoutes } from './shiping-request.routes';

@NgModule({
  declarations: [ShippingRequestComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(shippingRequestRoutes)
  ],
  exports: [],
})
export class ShippingRequestModule { }
