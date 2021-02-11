import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ReceivedMarkComponent } from './received-mark.component';
import { receivedMarkRoutes } from './received-mark.routes';

@NgModule({
  declarations: [ReceivedMarkComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(receivedMarkRoutes)],
  exports: [],
})
export class ReceivedMarkModule {}
