import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BrandRoutingModule } from './brand-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { BrandComponent } from './container/brand.component';


@NgModule({
  declarations: [BrandComponent],
  imports: [CommonModule, BrandRoutingModule, SharedModule],
  exports: [BrandComponent]
})
export class BrandModule { }
