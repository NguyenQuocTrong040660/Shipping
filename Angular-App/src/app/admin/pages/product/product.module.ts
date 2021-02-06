import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductRoutingModule } from './product-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { ProductComponent } from './product.component';

@NgModule({
  declarations: [ProductComponent],
  imports: [CommonModule, ProductRoutingModule, SharedModule],
  exports: [ProductComponent],
})
export class ProductModule {}
