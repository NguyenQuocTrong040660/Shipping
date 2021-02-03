import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PromotionRoutingModule } from './promotion-routing.module';
import { PromotionComponent } from './container/promotion.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [PromotionComponent],
  imports: [CommonModule, PromotionRoutingModule, SharedModule],
  exports: [PromotionComponent],
})
export class PromotionModule {}
