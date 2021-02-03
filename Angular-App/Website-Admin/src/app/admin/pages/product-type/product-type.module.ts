import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductTypeRoutingModule } from './product-type-routing.module';
import { ProductTypeComponent } from './container/product-type.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [ProductTypeComponent],
  imports: [CommonModule, ProductTypeRoutingModule, SharedModule],
  exports: [ProductTypeComponent],
})
export class ProductTypeModule {}
