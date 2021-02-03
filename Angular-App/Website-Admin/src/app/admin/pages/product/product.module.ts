import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './container/product.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [ProductComponent],
  imports: [CommonModule, ProductRoutingModule, SharedModule],
  exports: [ProductComponent],
})
export class ProductModule { }
