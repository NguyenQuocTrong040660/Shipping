import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ShippingPlanComponent } from './shipping-plan.component';
import { shippingPlanRoutes } from './shipping-plan.routes';

@NgModule({
  declarations: [ShippingPlanComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(shippingPlanRoutes)
  ],
  exports: [],
})
export class ShippingPlanModule {}
