import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserRole } from 'app/shared/constants/user-role.constants';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { MenuItem } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'admin-control-sidebar',
  templateUrl: './admin-control-sidebar.component.html',
  styleUrls: ['./admin-control-sidebar.component.scss'],
})
export class AdminControlSidebarComponent implements OnInit, OnDestroy {
  items: MenuItem[];
  user: ApplicationUser;
  private destroyed$ = new Subject<void>();

  constructor(private authenticationService: AuthenticationService) {}

  ngOnInit(): void {
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user) => {
      if (user) {
        this.user = user;
      }
    });

    this.items = [
      {
        label: 'Shipping',
        icon: 'pi pi-pw pi-file',
        items: [
          {
            label: 'Shipping Request',
            icon: 'pi pi-envelope',
            routerLink: '/shipping-request',
          },
          {
            label: 'Shipping Mark',
            icon: 'pi pi-tags',
            routerLink: '/shipping-mark',
          },
          {
            label: 'Shipping Mark Picking',
            icon: 'pi pi-th-large',
            routerLink: '/shipping-mark-picking',
          },
        ],
      },
      {
        label: 'Movement',
        icon: 'pi pi-pw pi-file',
        items: [
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
        ],
      },
      {
        label: 'Management',
        icon: 'pi pi-pw pi-file',
        items: [
          {
            label: 'Package Rule',
            icon: 'pi pi-list',
            routerLink: '/package-rule',
          },
          {
            label: 'Work Order',
            icon: 'pi pi-file-o',
            routerLink: '/work-order',
          },
          {
            label: 'Shipping Plan',
            icon: 'pi pi-calendar-times',
            routerLink: '/shipping-plan',
          },
        ],
      },
      {
        label: 'Setting',
        icon: 'pi pi-pw pi-file',
        visible: this._viewedByRoles([UserRole.SystemAdministrator, UserRole.ITAdministrator]),
        items: [
          {
            label: 'User',
            icon: 'pi pi-users',
            routerLink: '/user-management',
            visible: this._viewedByRoles([UserRole.SystemAdministrator, UserRole.ITAdministrator]),
          },
          {
            label: 'Configuration',
            icon: 'pi pi-cog',
            routerLink: '/configuration',
          },
        ],
      },
    ];
  }

  _viewedByRoles(roles: string[]): boolean {
    let haveAccess = false;
    this.user.roles.forEach((r) => {
      const result = roles.some((i) => i === r);

      if (result) {
        haveAccess = true;
      }
    });

    return haveAccess;
  }

  ngOnDestroy() {
    this.destroyed$.next();
    this.destroyed$.unsubscribe();
  }
}
