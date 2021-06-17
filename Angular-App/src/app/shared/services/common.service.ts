import { Injectable } from '@angular/core';
import { ApiException } from '../api-clients/shipping-app/shipping-app.client';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  constructor() {}

  getErrorMessagesFromResponse(errorInstance) {
    const msgs = [];

    if (errorInstance instanceof ApiException) {
      const response: any = JSON.parse(errorInstance.response);

      for (const prop in response.errors) {
        response.errors[prop].forEach((msg) => {
          msgs.push(msg);
        });
      }
    } else if (errorInstance && errorInstance.error instanceof ErrorEvent) {
      msgs.push('An unexpected server error occurred.');
    } else {
      for (const prop in errorInstance.errors) {
        errorInstance.errors[prop].forEach((msg) => {
          msgs.push(msg);
        });
      }
    }

    if (msgs.length === 0) {
      msgs.push('An unexpected server error occurred.');
    }

    return msgs;
  }
}
