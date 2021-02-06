import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'app/shared/shared.module';

import { CountryRoutingModule } from './country-routing.module';
import { CountryComponent } from './country.component';

@NgModule({
  declarations: [CountryComponent],
  imports: [CommonModule, CountryRoutingModule, SharedModule],
  exports: [CountryComponent]
})
export class CountryModule { }
