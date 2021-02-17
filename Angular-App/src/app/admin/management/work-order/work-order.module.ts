import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { WorkOrderComponent } from './work-order.component';
import { workOrderRoutes } from './work-order.routes';
import { WorkOrderCreateComponent } from './work-order-create/work-order-create.component';
import { WorkOrderEditComponent } from './work-order-edit/work-order-edit.component';

@NgModule({
  declarations: [WorkOrderComponent, WorkOrderCreateComponent, WorkOrderEditComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(workOrderRoutes)],
  exports: [WorkOrderComponent],
})
export class WorkOrderModule {}
