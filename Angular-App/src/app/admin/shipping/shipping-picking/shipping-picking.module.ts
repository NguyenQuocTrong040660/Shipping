import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShippingPickingRoutingModule } from './shipping-picking-routing.module';
import { ShippingPickingComponent } from './shipping-picking.component';
import { SharedModule } from 'app/shared/shared.module';
import { ShippingPickingDetailComponent } from './shipping-picking-detail/shipping-picking-detail.component';

@NgModule({
  declarations: [ShippingPickingComponent, ShippingPickingDetailComponent],
  imports: [CommonModule, SharedModule, ShippingPickingRoutingModule],
  exports: [ShippingPickingComponent],
})
export class ShippingPickingModule {}
