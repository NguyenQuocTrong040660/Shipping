import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'primeng/api';
import { ShippingMarkComponent } from './shipping-mark.component';
import { shippingMarkRoutes } from './shipping-mark.routes';

@NgModule({
  declarations: [ShippingMarkComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(shippingMarkRoutes)
  ],
  exports: [],
})
export class ShippingMarkModule {}
