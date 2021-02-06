import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilesManagementRoutingModule } from './files-management-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { FilesManagementComponent } from './files-management.component';

@NgModule({
  declarations: [FilesManagementComponent],
  imports: [CommonModule, FilesManagementRoutingModule, SharedModule],
  exports: [FilesManagementComponent],
})
export class FilesManagementModule {}
