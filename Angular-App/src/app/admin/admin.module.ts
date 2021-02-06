import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { AdminControlSidebarComponent } from './admin-control-sidebar/admin-control-sidebar.component';
import { AdminComponent } from './admin.component';
import { AdminFooterComponent } from './admin-footer/admin-footer.component';
import { AdminNavbarComponent } from './admin-navbar/admin-navbar.component';
import { adminRoutes } from './admin.routes';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    RouterModule.forChild(adminRoutes)],
  declarations: [
    AdminComponent,
    AdminControlSidebarComponent,
    AdminFooterComponent,
    AdminNavbarComponent],
  providers: [],
})
export class AdminModule {}
