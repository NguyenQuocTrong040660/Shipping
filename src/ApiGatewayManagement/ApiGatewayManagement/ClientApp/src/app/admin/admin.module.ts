import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { AdminFooterComponent } from './components/admin-footer/admin-footer.component';
import { AdminNavbarComponent } from './components/admin-navbar/admin-navbar.component';
import { AdminSidebarComponent } from './components/admin-sidebar/admin-sidebar.component';
import { AdminControlSidebarComponent } from './components/admin-control-sidebar/admin-control-sidebar.component';
import { AdminComponent } from './container/admin.component';
import { AdminRoutingModule } from './admin-routing.module';

@NgModule({
  imports: [CommonModule, RouterModule, AdminRoutingModule, SharedModule],
  declarations: [
    AdminComponent,
    AdminControlSidebarComponent,
    AdminFooterComponent,
    AdminNavbarComponent,
    AdminSidebarComponent,
  ],
})
export class AdminModule {}
