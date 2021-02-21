import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { MovementRequestComponent } from './movement-request.component';
import { movementRequestRoutes } from './movement-request.routes';
import { MovementRequestCreateComponent } from './movement-request-create/movement-request-create.component';
import { MovementRequestEditComponent } from './movement-request-edit/movement-request-edit.component';

@NgModule({
  declarations: [MovementRequestComponent, MovementRequestCreateComponent, MovementRequestEditComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(movementRequestRoutes)],
  exports: [],
})
export class MovementRequestModule {}
