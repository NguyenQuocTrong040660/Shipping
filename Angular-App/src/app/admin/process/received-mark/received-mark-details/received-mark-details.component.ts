import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ReceivedMarkPrintingModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { ApplicationUser } from 'app/shared/models/application-user';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-received-mark-details',
  templateUrl: './received-mark-details.component.html',
  styleUrls: ['./received-mark-details.component.scss'],
})
export class ReceivedMarkDetailsComponent implements OnInit {
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() user: ApplicationUser;
  @Input() canRePrint = false;
  @Input() receivedMarkPrintings: ReceivedMarkPrintingModel[] = [];

  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() printMarkEvent = new EventEmitter<any>();
  @Output() rePrintMarkEvent = new EventEmitter<any>();
  @Output() unstuffMarkEvent = new EventEmitter<any>();

  cols: any[] = [];
  TypeColumn = TypeColumn;

  constructor(private confirmationService: ConfirmationService) {}

  ngOnInit(): void {
    this.cols = [
      { header: 'Package Sequence', field: 'sequence', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Qty/ Pkg', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Printed By', field: 'printingBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Printed Time', field: 'printingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: 'actions', width: WidthColumn.NormalColumn, type: TypeColumn.ExpandColumn },
    ];
  }

  hideDialog() {
    this.hideDialogEvent.emit();
  }

  printMark() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to print mark for this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.printMarkEvent.emit()
    });
  }

  getHelpText() {
    return this.canRePrint ? '' : 'Received Mark has been printed. Please contact your manager to re-print';
  }

  handleRePrintMark(receivedMarkPrinting: ReceivedMarkPrintingModel) {
    if (!this.canRePrint) {
      return;
    }

    this.confirmationService.confirm({
      message: 'Are you sure you want to re-print this mark?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.rePrintMarkEvent.emit(receivedMarkPrinting),
    });
  }

  handleUnStuffMark(receivedMarkPrinting: ReceivedMarkPrintingModel) {
    this.unstuffMarkEvent.emit(receivedMarkPrinting);
  }
}