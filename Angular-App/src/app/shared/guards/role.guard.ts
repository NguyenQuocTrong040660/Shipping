import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserClient } from '../api-clients/user/user.client';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private router: Router, private userClient: UserClient) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const roles = route.data.roles as Array<string>;

    this.userClient.apiUserUserInfo().subscribe((user) => {
      if (user) {
        let haveAccess = false;

        user.roles.forEach((role) => {
          const result = roles.some((i) => i === role);
          if (result) {
            haveAccess = true;
          }
        });

        if (haveAccess) {
          return true;
        } else {
          this.router.navigateByUrl('/not-found');
          return false;
        }
      } else {
        this.router.navigateByUrl('/login');
        return false;
      }
    });

    return true;
  }
}
