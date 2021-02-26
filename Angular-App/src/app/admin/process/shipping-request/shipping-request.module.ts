import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ShippingRequestComponent } from './shipping-request.component';
import { shippingRequestRoutes } from './shipping-request.routes';
import { ShippingRequestCreateComponent } from './shipping-request-create/shipping-request-create.component';

@NgModule({
  declarations: [ShippingRequestComponent, ShippingRequestCreateComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(shippingRequestRoutes)],
  exports: [ShippingRequestComponent, ShippingRequestCreateComponent],
})
export class ShippingRequestModule {}
