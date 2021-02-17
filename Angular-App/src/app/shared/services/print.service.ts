import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PrintService {
  isPrinting = false;
  componentSelector = '';

  constructor(private router: Router) { }

  printDocument(componentSelector: string) {
    this.isPrinting = true;
    this.componentSelector = componentSelector;
    this.router.navigate(['/' + this.componentSelector,
    {
      outlets: {
        'print': ['print']
      }
    }]);
  }

  onDataReady() {
    window.print();
    this.isPrinting = false;
    this.router.navigate(['/' + this.componentSelector, { outlets: { print: null } }]);
  }
}
