import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

declare const Swal: any;

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  constructor() {}

  public showErrorMessage(message, title) {
    Swal.fire({
      icon: 'error',
      title: title,
      text: message,
    });
  }

  public showInfoMessage(message, title) {
    Swal.fire({
      icon: 'info',
      title: title,
      text: message,
    });
  }

  public showSuccessMessage(message, title) {
    Swal.fire({
      icon: 'success',
      title: title,
      text: message,
    });
  }

  public showConfirmMessage() {
    return Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'info',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!',
    });
  }
}
