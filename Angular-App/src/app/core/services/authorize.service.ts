import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthorizeService {
  constructor() {}

  public isAuthenticated() {
    return !!localStorage.getItem('USERLOGGED');
  }
}
