import { Component, OnInit } from '@angular/core';

@Component({
  templateUrl: './movement-request.component.html',
  styleUrls: ['./movement-request.component.scss']
})
export class MovementRequestComponent implements OnInit {
  movementRequest: {id: string, name: string, note: string }[] = [];

  ngOnInit() {
    this.movementRequest = [
      {
        id: '1',
        name: 'Movement Request A',
        note: 'This is Movement Request A note'
      },
      {
        id: '2',
        name: 'Movement Request B',
        note: 'This is Movement Request B note'
      },
      {
        id: '3',
        name: 'Movement Request C',
        note: 'This is Movement Request C note'
      },
      {
        id: '4',
        name: 'Movement Request D',
        note: 'This is Movement Request D note'
      },
      {
        id: '5',
        name: 'Movement Request E',
        note: 'This is Movement Request E note'
      },
      {
        id: '6',
        name: 'Movement Request F',
        note: 'This is Movement Request F note'
      }
    ];
  }
}
