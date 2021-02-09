import { Component, OnInit } from '@angular/core';

@Component({
  templateUrl: './shipping-mark.component.html',
  styleUrls: ['./shipping-mark.component.scss']
})
export class ShippingMarkComponent implements OnInit {
  shippingMark: {id: string, name: string, note: string }[] = [];

  ngOnInit() {
    this.shippingMark = [
      {
        id: '1',
        name: 'Shipping Mark A',
        note: 'This is Shipping Mark A note'
      },
      {
        id: '2',
        name: 'Shipping Mark B',
        note: 'This is Shipping Mark B note'
      },
      {
        id: '3',
        name: 'Shipping Mark C',
        note: 'This is Shipping Mark C note'
      },
      {
        id: '4',
        name: 'Shipping Mark D',
        note: 'This is Shipping Mark D note'
      },
      {
        id: '5',
        name: 'Shipping Mark E',
        note: 'This is Shipping Mark E note'
      },
      {
        id: '6',
        name: 'Shipping Mark F',
        note: 'This is Shipping Mark F note'
      }
    ];
  }
}
