import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from 'app/shared/shared.module';
import { ProductComponent } from './product.component';
import { RouterModule } from '@angular/router';
import { productRoutes } from './product.routes';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(productRoutes)
  ],
  declarations: [ProductComponent],
  exports: [],
})
export class ProductModule {}
