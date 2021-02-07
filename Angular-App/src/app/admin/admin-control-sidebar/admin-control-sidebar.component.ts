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
        label: 'Business',
        icon: 'pi pi-pw pi-file',
        items: [
          {
            label: 'Shipping Plan',
            icon: 'pi pi-calendar-times',
            routerLink: '',
          },
          {
            label: 'Shipping Request',
            icon: 'pi pi-envelope',
            routerLink: '',
          },
          {
            label: 'Movement Request',
            icon: 'pi pi-envelope',
            routerLink: '',
          },
          {
            label: 'Received Mark',
            icon: 'pi pi-tags',
            routerLink: '',
          },
          {
            label: 'Shipping Mark',
            icon: 'pi pi-tags',
            routerLink: '',
          },
        ],
      },
      {
        label: 'Management',
        icon: 'pi pi-pw pi-file',
        items: [
          {
            label: 'Users',
            icon: 'pi pi-users',
            routerLink: '',
          },
          {
            label: 'Product',
            icon: 'pi pi-list',
            routerLink: '',
          },
          {
            label: 'Product Category',
            icon: 'pi pi-list',
            routerLink: '',
          },
          {
            label: 'Work Order',
            icon: 'pi pi-file-o',
            routerLink: '',
          },
        ],
      },
      {
        label: 'Settings',
        icon: 'pi pi-pw pi-file',
        items: [
          {
            label: 'Imports Data',
            icon: 'pi pi-cloud-upload',
            routerLink: '',
          },
          {
            label: 'Export Templates',
            icon: 'pi pi-folder-open',
            routerLink: '',
          },
          {
            label: 'Configuration',
            icon: 'pi pi-cog',
            routerLink: '',
          },
        ],
      },
    ];
  }
}
