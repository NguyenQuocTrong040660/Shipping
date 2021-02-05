import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'app/shared/shared.module';
import { RegisterRoutingModule } from './register-routing.module';
import { RegisterComponent } from './container/register.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
  declarations: [RegisterComponent, RegisterComponent],
  imports: [CommonModule, RegisterRoutingModule, SharedModule, ReactiveFormsModule, FormsModule],
})
export class RegisterModule {}
