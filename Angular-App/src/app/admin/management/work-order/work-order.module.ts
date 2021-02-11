import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { WorkOrderComponent } from './work-order.component';
import { workOrderRoutes } from './work-order.routes';

@NgModule({
  declarations: [WorkOrderComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(workOrderRoutes)],
  exports: [],
})
export class WorkOrderModule {}
