import { Injectable } from '@angular/core';
import { UserClient, LoginRequest, RegisterRequest, RefreshTokenRequest } from 'app/api-clients/user-client';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private client: UserClient) {}

  login(req: LoginRequest) {
    return this.client.apiUserUserLogin(req);
  }

  register(req: RegisterRequest) {
    return this.client.apiUserUserRegister(req);
  }

  logout() {
    return this.client.apiUserUserLogout();
  }

  userInfo() {
    return this.client.apiUserUserInfo();
  }

  refreshToken(req: RefreshTokenRequest) {
    return this.client.apiUserUserRefreshToken(req);
  }
}
