import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ReceivedMarkPrintingModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { EventType } from 'app/shared/enumerations/import-event-type.enum';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-received-mark-merge',
  templateUrl: './received-mark-merge.component.html',
  styleUrls: ['./received-mark-merge.component.scss'],
})
export class ReceivedMarkMergeComponent implements OnInit {
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() receivedMarkPrintings: ReceivedMarkPrintingModel[] = [];
  selectedReceivedMarkPrintings: ReceivedMarkPrintingModel[] = [];

  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() mergeMarkEvent = new EventEmitter<any>();

  cols: any[] = [];
  TypeColumn = TypeColumn;

  constructor(private confirmationService: ConfirmationService) {}

  ngOnInit(): void {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Package Sequence', field: 'sequence', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Qty/ Pkg', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Printed By', field: 'printingBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Printed Time', field: 'printingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];
  }

  hideDialog() {
    this.hideDialogEvent.emit(EventType.RefreshData);
  }

  mergeReceivedMarks() {
    this.confirmationService.confirm({
      message: 'Do you want to merge all these received marks?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.mergeMarkEvent.emit(this.selectedReceivedMarkPrintings),
    });
  }
}
