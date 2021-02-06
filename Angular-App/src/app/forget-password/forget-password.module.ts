import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ForgetPasswordRoutingModule } from './forget-password-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ForgetPasswordComponent } from './forget-password.component';

@NgModule({
  declarations: [ForgetPasswordComponent],
  imports: [CommonModule, ForgetPasswordRoutingModule, ReactiveFormsModule, FormsModule],
})
export class ForgetPasswordModule {}
