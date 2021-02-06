import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';

export enum States {
  logged = 'logged',
  accessToken = 'accessToken',
}

export interface AppState {
  logged: boolean;
  accessToken: string;
}

export const InitialState: AppState = {
  logged: false,
  accessToken: '',
};

@Injectable({
  providedIn: 'root',
})
export class StateService {
  private state: AppState;
  private stateSubject: BehaviorSubject<AppState>;

  constructor() {
    const accessToken = localStorage.getItem('accessToken');

    this.state = {
      accessToken: accessToken,
      logged: false,
    };

    this.stateSubject = new BehaviorSubject<AppState>(this.state);
  }

  setState<T extends keyof AppState>(key: T, value: AppState[T], local = true) {
    this.saveState(key, value, local);
    this.state[key] = value;
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

  saveState(key, value, local) {
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
}
