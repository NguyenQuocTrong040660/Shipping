import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilesManagementRoutingModule } from './files-management-routing.module';
import { FilesManagementComponent } from './container/files-management.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [FilesManagementComponent],
  imports: [CommonModule, FilesManagementRoutingModule, SharedModule],
  exports: [FilesManagementComponent],
})
export class FilesManagementModule {}
