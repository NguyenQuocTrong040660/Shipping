import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ReceivedMarkComponent } from './received-mark.component';
import { receivedMarkRoutes } from './received-mark.routes';
import { ReceivedMarkUnstuffComponent } from './received-mark-unstuff/received-mark-unstuff.component';

@NgModule({
  declarations: [ReceivedMarkComponent, ReceivedMarkUnstuffComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(receivedMarkRoutes)],
  exports: [ReceivedMarkComponent, ReceivedMarkUnstuffComponent],
})
export class ReceivedMarkModule {}
