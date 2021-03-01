import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { LazyLoadEvent } from 'primeng/api';

@Component({
  selector: 'app-history-dialog',
  templateUrl: './history-dialog.component.html',
  styleUrls: ['./history-dialog.component.scss'],
})
export class HistoryDialogComponent implements OnInit, OnChanges {
  @Input() isShowDialog: boolean;
  @Input() type: HistoryDialogType;
  @Output() hideDialogEvent = new EventEmitter<any>();

  searchText: string;
  cols: any[] = [];
  TypeColumn = TypeColumn;
  loading: boolean;

  tittle: string;
  histories: History[] = [];

  constructor() { }

  ngOnInit() {
    this.cols = [
      { header: 'Id', field: 'id', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Ref Id', field: 'refId', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Action Type', field: 'actionType', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Old Value', field: 'oldValue', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'New Value', field: 'newValue', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];
  }

  ngOnChanges() {
    this.tittle = HistoryDialogType[this.type];
  }

  hideDialog() {
    this.searchText = '';
    this.histories = [];
    this.isShowDialog = false;
    this.hideDialogEvent.emit();
  }

  search() {
    this.loading = true;

    switch (this.type) {
      case HistoryDialogType.WorkOrder: {
        setTimeout(() => {
          this.histories = [
            {
              id: '1',
              refId: '1a',
              actionType: 'create',
              oldValue: 'oldValue',
              newValue: 'newValue',
              lastModifiedBy: 'User A',
              lastModified: new Date(),
            },
          ];
          this.loading = false;
        }, 1000);

        break;
      }
      case HistoryDialogType.ShippingPlan: {
        break;
      }
      case HistoryDialogType.ShippingRequest: {
        break;
      }
      case HistoryDialogType.MovementRequest: {
        break;
      }
      case HistoryDialogType.ReceivedMark: {
        break;
      }
      case HistoryDialogType.ShippingMark: {
        break;
      }
    }
  }
}

export interface History {
  id: string;
  refId: string;
  actionType: string;
  oldValue: string;
  newValue: string;
  lastModifiedBy: string;
  lastModified: Date;
}
