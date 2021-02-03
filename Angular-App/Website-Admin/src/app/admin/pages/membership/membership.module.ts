import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MemberShipRoutingModule } from './membership-routing.module';
import { MemberShipComponent } from './container/membership.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [MemberShipComponent],
  imports: [CommonModule, MemberShipRoutingModule, SharedModule],
  exports: [MemberShipComponent],
})
export class MemberShipModule {}
