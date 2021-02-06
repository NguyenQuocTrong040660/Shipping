import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AttachmentTypesManagementRoutingModule } from './attachment-types-management-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { AttachmentTypesManagementComponent } from './attachment-types-management.component';

@NgModule({
  declarations: [AttachmentTypesManagementComponent],
  imports: [CommonModule, AttachmentTypesManagementRoutingModule, SharedModule],
  exports: [AttachmentTypesManagementComponent],
})
export class AttachmentTypesManagementModule {}
