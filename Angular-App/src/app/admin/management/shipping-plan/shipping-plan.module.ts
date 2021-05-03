import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ShippingPlanComponent } from './shipping-plan.component';
import { shippingPlanRoutes } from './shipping-plan.routes';
import { ShippingPlanCreateComponent } from './shipping-plan-create/shipping-plan-create.component';
import { ShippingPlanEditComponent } from './shipping-plan-edit/shipping-plan-edit.component';

@NgModule({
  declarations: [ShippingPlanComponent, ShippingPlanCreateComponent, ShippingPlanEditComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(shippingPlanRoutes)],
  exports: [ShippingPlanComponent, ShippingPlanCreateComponent],
})
export class ShippingPlanModule { }
