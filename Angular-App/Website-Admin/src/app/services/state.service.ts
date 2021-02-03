import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';

export enum States {
  logged = 'logged',
  accessToken = 'accessToken',

  companyIndex = 'companyIndex',
}

export interface AppState {
  logged: boolean;
  accessToken: string;

  companyIndex: number;
}

export const initialState: AppState = {
  logged: false,
  accessToken: '',

  companyIndex: 0,
};

@Injectable({
  providedIn: 'root',
})
export class StateService {
  private state: AppState;
  private stateSubject: BehaviorSubject<AppState>;

  constructor() {
    const companyIndex = localStorage.getItem('companyIndex') || sessionStorage.getItem('companyIndex');
    const accessToken = localStorage.getItem('accessToken');
    this.state = {
      companyIndex: parseInt(companyIndex, 10) >= 0 ? parseInt(companyIndex, 10) : 0,
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

  resetStates() {
    localStorage.removeItem('companyIndex');
    this.stateSubject.next(initialState);
  }

  resetToken() {
    localStorage.removeItem('accessToken');
    this.stateSubject.next(initialState);
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
