import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { userRoutes } from './user.routes';
import { UserComponent } from './user.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  imports: [CommonModule, FormsModule, ReactiveFormsModule, SharedModule, RouterModule.forChild(userRoutes)],
  declarations: [UserComponent],
  exports: [UserComponent],
})
export class UserModule {}
