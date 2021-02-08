import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from 'app/shared/shared.module';
import { ProductTypeComponent } from './product-type.component';
import { RouterModule } from '@angular/router';
import { productTypeRoutes } from './product-type.routes';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(productTypeRoutes)],
  declarations: [
    ProductTypeComponent],
  exports: [],
})
export class ProductTypeModule {}
