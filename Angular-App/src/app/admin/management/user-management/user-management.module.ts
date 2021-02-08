import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { userRoutes } from './user-management.routes';
import { SharedModule } from 'app/shared/shared.module';
import { UserManagementComponent } from './user-management.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(userRoutes)],
  declarations: [
    UserManagementComponent],
  exports: [],
})
export class UserManagementModule {}
