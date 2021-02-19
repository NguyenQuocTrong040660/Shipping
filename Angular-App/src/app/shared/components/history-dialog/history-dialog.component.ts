import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { HistoryDialogType } from 'app/shared/enums/history-dialog-type.enum';

@Component({
  selector: 'app-history-dialog',
  templateUrl: './history-dialog.component.html',
  styleUrls: ['./history-dialog.component.scss']
})
export class HistoryDialogComponent implements OnInit, OnChanges {
  @Input() isShowDialog: boolean;
  @Input() type: HistoryDialogType;
  @Output() hideDialogEvent = new EventEmitter<any>();

  tittle: string;

  constructor() { }

  ngOnInit() {

  }

  ngOnChanges() {
    this.tittle = HistoryDialogType[this.type];
  }

  hideDialog() {
    this.isShowDialog = false;
    this.hideDialogEvent.emit();
  }
}
