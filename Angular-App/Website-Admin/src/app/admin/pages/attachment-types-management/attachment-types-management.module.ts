import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AttachmentTypesManagementRoutingModule } from './attachment-types-management-routing.module';
import { AttachmentTypesManagementComponent } from './container/attachment-types-management.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [AttachmentTypesManagementComponent],
  imports: [CommonModule, AttachmentTypesManagementRoutingModule, SharedModule],
  exports: [AttachmentTypesManagementComponent],
})
export class AttachmentTypesManagementModule {}
