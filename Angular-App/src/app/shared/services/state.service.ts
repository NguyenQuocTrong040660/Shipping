import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';

export enum States {
  AccessToken = 'accessToken',
  RefreshToken = 'refreshToken',
  LoginEvent = 'loginEvent',
}

export interface AppState {
  refreshToken: string;
  accessToken: string;
  loginEvent: string;
}

export const InitialState: AppState = {
  accessToken: '',
  refreshToken: '',
  loginEvent: '',
};

@Injectable({
  providedIn: 'root',
})
export class StateService {
  private state: AppState;
  private stateSubject: BehaviorSubject<AppState>;

  constructor() {
    const accessToken = localStorage.getItem(States.AccessToken);
    const refreshToken = localStorage.getItem(States.RefreshToken);
    const loginEvent = localStorage.getItem(States.LoginEvent);

    this.state = {
      accessToken,
      refreshToken,
      loginEvent,
    };

    this.stateSubject = new BehaviorSubject<AppState>(this.state);
  }

  setState<T extends keyof AppState>(key: T, value: AppState[T], local = true) {
    this.saveState(key, value, local);
    this.state[key] = value;
    this.stateSubject.next(this.state);
  }

  resetState<T extends keyof AppState>(key: T, local = true) {
    this.removeState(key, local);
    this.state[key] = InitialState[key];
    this.stateSubject.next(this.state);
  }

  selectState<R>(selector: (state: AppState) => R): Observable<R> {
    return this.stateSubject.pipe(map(selector), distinctUntilChanged());
  }

  select<T extends keyof AppState>(key: T) {
    return this.stateSubject.value[key];
  }

  resetToken() {
    localStorage.removeItem('accessToken');
    this.stateSubject.next(InitialState);
  }

  saveState(key: string, value, local: boolean) {
    if (typeof value === 'boolean') {
      return;
    }
    if (typeof value === 'object') {
      value = JSON.stringify(value);
    }

    if (local) {
      localStorage.setItem(key, value);
    } else {
      sessionStorage.setItem(key, value);
    }
  }

  removeState(key: string, local: boolean) {
    if (local) {
      localStorage.removeItem(key);
    } else {
      sessionStorage.removeItem(key);
    }
  }
}
