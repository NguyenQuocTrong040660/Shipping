import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  PrintShippingMarkRequest,
  RePrintShippingMarkRequest,
  ShippingMarkClients,
  ShippingMarkModel,
  ShippingMarkPrintingModel,
  ShippingMarkShippingModel,
} from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { EventType } from 'app/shared/enumerations/import-event-type.enum';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-shipping-picking',
  templateUrl: './shipping-picking.component.html',
  styleUrls: ['./shipping-picking.component.scss'],
})
export class ShippingPickingComponent implements OnInit, OnDestroy {
  title = 'Shipping Mark';
  titleDialog = '';

  user: ApplicationUser;
  shippingMarks: ShippingMarkModel[] = [];
  selectedShippingMark: ShippingMarkModel;

  currentShippingMark: ShippingMarkModel;
  currentShippingMarkShippingModel: ShippingMarkShippingModel;

  shippingMarkPrintings: ShippingMarkPrintingModel[] = [];
  shippingMarkShippings: ShippingMarkShippingModel[] = [];

  canRePrint = false;

  isEdit = false;
  isShowDialog = false;
  isShowDialogDetail = false;
  isShowDialogHistory = false;

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  expandedItems: any[] = [];
  printData: any;

  private destroyed$ = new Subject<void>();

  constructor(
    public printService: PrintService,
    private shippingMarkClients: ShippingMarkClients,
    private notificationService: NotificationService,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit() {
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));
    this.canRePrint = this.printService.canRePrint(this.user);

    this.cols = [
      //{ header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initShippingMarks();
  }

  initShippingMarks() {
    this.expandedItems = [];

    this.shippingMarkClients
      .getShippingMarks()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingMarks = i),
        (_) => (this.shippingMarks = [])
      );
  }

  hideDialog(eventType = EventType.HideDialog) {
    if (eventType == EventType.RefreshData && this.selectedShippingMark) {
      this.getShippingMarkMovementRequestsFullInfo(this.selectedShippingMark);
    }

    this.isShowDialog = false;
    this.isShowDialogHistory = false;
    this.isShowDialogDetail = false;
  }

  onPrint() {
    this.printService.printDocument('shipping-mark-picking');
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  printShippingMark() {
    if (!this.currentShippingMark || !this.currentShippingMarkShippingModel) {
      return;
    }

    const requestPrint: PrintShippingMarkRequest = {
      productId: this.currentShippingMarkShippingModel.productId,
      shippingMarkId: this.currentShippingMark.id,
      printedBy: this.user.userName,
    };

    this.shippingMarkClients
      .printShippingMark(requestPrint)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result) {
            this.printData = result;
            this.onPrint();
            this.reLoadShippingMarkPrintings(this.currentShippingMark.id, this.currentShippingMarkShippingModel.productId);
          } else {
            this.notificationService.error('Print Shipping Mark Failed. Please try again');
          }
        },
        (_) => this.notificationService.error('Print Shipping Mark Failed. Please try again')
      );
  }

  handleRePrintMark(item: ShippingMarkPrintingModel) {
    if (!item) {
      return;
    }

    const request: RePrintShippingMarkRequest = {
      shippingMarkPrintingId: item.id,
      rePrintedBy: this.user.userName,
    };

    this.shippingMarkClients
      .rePrintShippingMark(request)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result) {
            this.printData = result;
            this.onPrint();
            this.reLoadShippingMarkPrintings(this.currentShippingMark.id, this.currentShippingMarkShippingModel.productId);
          } else {
            this.notificationService.error('Print Shipping Mark Failed. Please try again');
          }
        },
        (_) => this.notificationService.error('Print Shipping Mark Failed. Please try again')
      );
  }

  getShippingMarkMovementRequestsFullInfo(item: ShippingMarkModel) {
    this.selectedShippingMark = item;
    const shippingMark = this.shippingMarks.find((i) => i.id === item.id);

    this.shippingMarkClients
      .getShippingMarkShippingsFullInfo(item.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (shippingMarkShippings) => {
          shippingMark.shippingMarkShippings.forEach((item) => {
            const shippingMarkShipping = shippingMarkShippings.find((i) => i.shippingMarkId === item.shippingMarkId && i.productId == item.productId);

            item.product = shippingMarkShipping.product;
            item.totalPackage = shippingMarkShipping.totalPackage;
            item.totalQuantityPrinted = shippingMarkShipping.totalQuantityPrinted;
            item.totalQuantity = shippingMarkShipping.totalQuantity;
          });
        },
        (_) => {}
      );
  }

  showDetailShippingMarkSummary(shippingMark: ShippingMarkModel, shippingMarkShippingModel: ShippingMarkShippingModel) {
    if (!shippingMark || !shippingMarkShippingModel) {
      return;
    }

    this.currentShippingMark = shippingMark;
    this.currentShippingMarkShippingModel = shippingMarkShippingModel;

    this.shippingMarkPrintings = [];
    this.shippingMarkClients
      .getShippingMarkPrintings(shippingMark.id, shippingMarkShippingModel.productId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (shippingMarkPrintings) => {
          this.shippingMarkPrintings = shippingMarkPrintings;
          this.isShowDialogDetail = true;
          this.titleDialog = `Product Number: ${shippingMarkShippingModel.product.productNumber} - ${shippingMarkShippingModel.product.productName}`;
        },
        (_) => this.notificationService.error('Failed to show detail')
      );
  }

  reLoadShippingMarkPrintings(shippingMarkId: number, productId: number) {
    this.shippingMarkPrintings = [];
    this.shippingMarkClients
      .getShippingMarkPrintings(shippingMarkId, productId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (shippingMarkPrintings) => (this.shippingMarkPrintings = shippingMarkPrintings),
        (_) => this.notificationService.error('Failed to show detail')
      );
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
