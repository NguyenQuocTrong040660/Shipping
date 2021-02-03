import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ForgetPasswordRoutingModule } from './forget-password-routing.module';
import { ForgetPasswordComponent } from './container/forget-password.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ForgetPasswordComponent, ForgetPasswordComponent],
  imports: [CommonModule, ForgetPasswordRoutingModule, ReactiveFormsModule, FormsModule],
})
export class ForgetPasswordModule {}
