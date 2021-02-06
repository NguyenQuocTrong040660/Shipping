import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'admin-control-sidebar',
  templateUrl: './admin-control-sidebar.component.html',
  styleUrls: ['./admin-control-sidebar.component.scss'],
})
export class AdminControlSidebarComponent implements OnInit {
  items: MenuItem[];

  constructor() {}

  ngOnInit(): void {
    this.items = [
      {
        label: 'Quản lý tập tin',
        icon: 'pi pi-pw pi-file',
        items: [
          {
            label: 'Danh sách hình ảnh',
            icon: 'pi pi-fw pi-images',
            routerLink: '/files-management/hinh-anh',
          },
          { separator: true },
          {
            label: 'Danh sách video',
            icon: 'pi pi-fw pi-video',
            routerLink: '/files-management/video',
          },
        ],
      },

      {
        label: 'Cấu hình',
        icon: 'pi pi-fw pi-cog',
        items: [
          {
            label: 'Loại tập tin',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/attachment-types-management',
          },
        ],
      },
      {
        label: 'Quản Lý Sản Phẩm',
        icon: 'fas fa-cubes',
        items: [
          {
            label: 'Danh Mục Sản Phẩm',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/product',
          },
          {
            label: 'Loại Sản Phẩm',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/product-type',
          },

          {
            label: 'Quốc Gia',
            icon: 'pi pi-fw pi-pencil',
            routerLink: '/country',
          },
        ],
      },
    ];
  }
}
