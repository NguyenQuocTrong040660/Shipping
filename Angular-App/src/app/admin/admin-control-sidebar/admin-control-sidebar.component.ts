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
            routerLink: '/shipping-plan',
          },
          {
            label: 'Shipping Request',
            icon: 'pi pi-envelope',
            routerLink: '/shipping-request',
          },
          {
            label: 'Movement Request',
            icon: 'pi pi-envelope',
            routerLink: '/movement-request',
          },
          {
            label: 'Received Mark',
            icon: 'pi pi-tags',
            routerLink: '/received-mark',
          },
          {
            label: 'Shipping Mark',
            icon: 'pi pi-tags',
            routerLink: '/shipping-mark',
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
            routerLink: '/user-management',
          },
          {
            label: 'Product',
            icon: 'pi pi-list',
            routerLink: '/product',
          },
          {
            label: 'Product Category',
            icon: 'pi pi-list',
            routerLink: '/product-type',
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
