import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'primeng/api';
import { MovementRequestComponent } from './movement-request.component';
import { movementRequestRoutes } from './movement-request.routes';

@NgModule({
  declarations: [MovementRequestComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(movementRequestRoutes)
  ],
  exports: [],
})
export class MovementRequestModule {}
