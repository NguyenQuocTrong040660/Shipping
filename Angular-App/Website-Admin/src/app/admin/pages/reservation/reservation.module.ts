import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReservationRoutingModule } from './reservation-routing.module';
import { ReservationComponent } from './container/reservation.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [ReservationComponent],
  imports: [CommonModule, ReservationRoutingModule, SharedModule],
  exports: [ReservationComponent],
})
export class ReservationModule {}
