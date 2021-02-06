import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AttachmentTypesManagementComponent } from './attachment-types-management.component';

const routes: Routes = [
  {
    path: '',
    component: AttachmentTypesManagementComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AttachmentTypesManagementRoutingModule {}
