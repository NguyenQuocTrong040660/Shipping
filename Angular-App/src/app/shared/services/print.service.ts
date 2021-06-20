import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Roles } from '../enumerations/roles.enum';
import { ApplicationUser } from '../models/application-user';

@Injectable({
  providedIn: 'root',
})
export class PrintService {
  isPrinting = false;
  componentSelector = '';

  constructor(private router: Router) {}

  printDocument(componentSelector: string) {
    this.isPrinting = true;
    this.componentSelector = componentSelector;
    this.router.navigate([
      '/' + this.componentSelector,
      {
        outlets: {
          print: ['print'],
        },
      },
    ]);
  }

  onDataReady() {
    window.print();
    this.isPrinting = false;
    this.router.navigate(['/' + this.componentSelector, { outlets: { print: null } }]);
  }

  canRePrint(user: ApplicationUser) {
    if (user && user.roles && user.roles.length > 0 && user.roles.includes(Roles.WarehouseSupervisor)) {
      return true;
    }

    return false;
  }
}
