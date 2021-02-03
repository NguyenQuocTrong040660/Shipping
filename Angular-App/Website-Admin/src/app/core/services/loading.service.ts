import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private $isLoading: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public IsLoading: Observable<boolean> = this.$isLoading.asObservable();

  constructor() {
    console.log('Init Loading Service');
  }

  showLoading() {
    this.$isLoading.next(true);
  }

  hideLoading() {
    this.$isLoading.next(false);
  }
}
