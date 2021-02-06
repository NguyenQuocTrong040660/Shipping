import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FilesManagementComponent } from './files-management.component';

const routes: Routes = [
  {
    path: '',
    component: FilesManagementComponent,
  },
  {
    path: 'hinh-anh',
    component: FilesManagementComponent,
  },
  {
    path: 'video',
    component: FilesManagementComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FilesManagementRoutingModule {}
