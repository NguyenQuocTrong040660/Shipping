import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ShippingMarkComponent } from './shipping-mark.component';
import { shippingMarkRoutes } from './shipping-mark.routes';
import { ShippingMarkCreateComponent } from './shipping-mark-create/shipping-mark-create.component';
import { ShippingMarkDetailsComponent } from './shipping-mark-details/shipping-mark-details.component';
import { ShippingMarkEditComponent } from './shipping-mark-edit/shipping-mark-edit.component';

@NgModule({
  declarations: [ShippingMarkComponent, ShippingMarkCreateComponent, ShippingMarkDetailsComponent, ShippingMarkEditComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(shippingMarkRoutes)],
  exports: [ShippingMarkComponent],
})
export class ShippingMarkModule {}
