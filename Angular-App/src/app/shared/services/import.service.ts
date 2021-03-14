import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { EventType } from '../enumerations/import-event-type.enum';

@Injectable({
  providedIn: 'root',
})
export class ImportService {
  private _event = new BehaviorSubject<EventType>(null);
  private event$: Observable<EventType> = this._event.asObservable();

  constructor() {}

  getEvent() {
    return this.event$;
  }

  dispactEvent(event: EventType) {
    this._event.next(event);
  }
}
