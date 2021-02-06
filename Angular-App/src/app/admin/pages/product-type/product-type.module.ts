import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductTypeRoutingModule } from './product-type-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { ProductTypeComponent } from './product-type.component';

@NgModule({
  declarations: [ProductTypeComponent],
  imports: [CommonModule, ProductTypeRoutingModule, SharedModule],
  exports: [ProductTypeComponent],
})
export class ProductTypeModule {}
