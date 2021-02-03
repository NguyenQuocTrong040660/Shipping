import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AttachmentTypesManagementComponent } from './container/attachment-types-management.component';

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
