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
        label: 'Process',
        icon: 'pi pi-pw pi-file',
        items: [
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
            label: 'Work Order',
            icon: 'pi pi-file-o',
            routerLink: '',
          },
          {
            label: 'Shipping Plan',
            icon: 'pi pi-calendar-times',
            routerLink: '/shipping-plan',
          },
        ],
      },
      {
        label: 'Settings',
        icon: 'pi pi-pw pi-file',
        items: [
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
