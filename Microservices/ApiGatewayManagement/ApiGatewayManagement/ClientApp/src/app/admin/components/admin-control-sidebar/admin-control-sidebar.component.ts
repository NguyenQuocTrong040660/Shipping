import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'admin-control-sidebar',
  templateUrl: './admin-control-sidebar.component.html',
  styleUrls: ['./admin-control-sidebar.component.scss'],
})
export class AdminControlSidebarComponent implements OnInit {
  items: MenuItem[];

  constructor(private router: Router) {}

  handleRouting(url) {
    if (!!url === false) return;

    return this.router.navigateByUrl(url);
  }

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
        label: 'Quản lý thông tin',
        icon: 'pi pi-fw pi-pencil',
        items: [
          {
            label: 'Video Embed',
            icon: 'pi pi-fw pi-globe',
            routerLink: '/youtube-embed-management',
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
    ];
  }
}
