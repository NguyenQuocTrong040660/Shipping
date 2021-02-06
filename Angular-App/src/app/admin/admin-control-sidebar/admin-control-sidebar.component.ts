import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'admin-control-sidebar',
  templateUrl: './admin-control-sidebar.component.html',
  styleUrls: ['./admin-control-sidebar.component.scss'],
})
export class AdminControlSidebarComponent implements OnInit {
  items: MenuItem[];

  constructor() { }

  ngOnInit(): void {
    this.items = [
      {
        label: 'Danh mục sản phẩm',
        icon: 'pi pi-pw pi-file',
        items: [
          {
            label: 'Danh sách Receive Mark',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/files-management/hinh-anh',
          },
          {
            label: 'Danh sách Shipping Mark',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/files-management/video',
          },
          {
            label: 'Danh sách Shipping Plan',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/files-management/video',
          },
          {
            label: 'Danh sách Shipping Order',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/files-management/video',
          },
          {
            label: 'Danh sách Move Orders',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/files-management/video',
          },
        ],
      }
    ];
  }
}
