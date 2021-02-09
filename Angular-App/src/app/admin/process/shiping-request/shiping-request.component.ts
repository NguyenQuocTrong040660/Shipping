import { Component, OnInit } from '@angular/core';

@Component({
  templateUrl: './shiping-request.component.html',
  styleUrls: ['./shiping-request.component.scss']
})
export class ShippingRequestComponent implements OnInit {
  shippingRequest: {id: string, name: string, note: string }[] = [];

  ngOnInit() {
    this.shippingRequest = [
      {
        id: '1',
        name: 'Shipping Request A',
        note: 'This is Shipping Request A note'
      },
      {
        id: '2',
        name: 'Shipping Request B',
        note: 'This is Shipping Request B note'
      },
      {
        id: '3',
        name: 'Shipping Request C',
        note: 'This is Shipping Request C note'
      },
      {
        id: '4',
        name: 'Shipping Request D',
        note: 'This is Shipping Request D note'
      },
      {
        id: '5',
        name: 'Shipping Request E',
        note: 'This is Shipping Request E note'
      },
      {
        id: '6',
        name: 'Shipping Request F',
        note: 'This is Shipping Request F note'
      }
    ];
  }
}
