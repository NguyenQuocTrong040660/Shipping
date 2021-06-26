import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ReceivedMarkPrintingModel } from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { EventType } from 'app/shared/enumerations/import-event-type.enum';
import { NotificationService } from 'app/shared/services/notification.service';
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
  @Input() selectedReceivedMarkPrintings: ReceivedMarkPrintingModel[] = [];

  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() mergeMarkEvent = new EventEmitter<any>();

  cols: any[] = [];
  TypeColumn = TypeColumn;

  constructor(private confirmationService: ConfirmationService, private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Package Sequence', field: 'sequence', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Qty/ Pkg', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Work Order', field: 'workOrder', subField: 'refId', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Printed By', field: 'printingBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Printed Time', field: 'printingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];
  }

  hideDialog() {
    this.hideDialogEvent.emit(EventType.RefreshData);
  }

  initPreviewReceivedMarkPrintings(receivedMarkPrintings: ReceivedMarkPrintingModel[]) {
    if (receivedMarkPrintings && receivedMarkPrintings.length === 0) {
      return [];
    }

    const receivedMarkPrinting = JSON.parse(JSON.stringify(receivedMarkPrintings[0]));

    receivedMarkPrinting.sequence = '#';
    receivedMarkPrinting.quantity = receivedMarkPrintings.reduce((i, j) => i + j.quantity, 0);

    return [receivedMarkPrinting];
  }

  mergeReceivedMarks() {
    if (this.selectedReceivedMarkPrintings && this.selectedReceivedMarkPrintings.length <= 1) {
      this.notificationService.error('Please select at least two packages to merge');
      return;
    }

    this.confirmationService.confirm({
      message: 'Do you want to merge all these received marks?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.mergeMarkEvent.emit(this.selectedReceivedMarkPrintings);
        this.selectedReceivedMarkPrintings = [];
      },
    });
  }
}
