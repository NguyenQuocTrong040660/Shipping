import { Component, OnInit } from '@angular/core';

@Component({
  templateUrl: './received-mark.component.html',
  styleUrls: ['./received-mark.component.scss']
})
export class ReceivedMarkComponent implements OnInit {
  receivedMark: {id: string, name: string, note: string }[] = [];

  ngOnInit() {
    this.receivedMark = [
      {
        id: '1',
        name: 'Received Mark A',
        note: 'This is Received Mark A note'
      },
      {
        id: '2',
        name: 'Received Mark B',
        note: 'This is Received Mark B note'
      },
      {
        id: '3',
        name: 'Received Mark C',
        note: 'This is Received Mark C note'
      },
      {
        id: '4',
        name: 'Received Mark D',
        note: 'This is Received Mark D note'
      },
      {
        id: '5',
        name: 'Received Mark E',
        note: 'This is Received Mark E note'
      },
      {
        id: '6',
        name: 'Received Mark F',
        note: 'This is Received Mark F note'
      }
    ];
  }
}
