import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  PrintShippingMarkRequest,
  RePrintShippingMarkRequest,
  ShippingMarkClients,
  ShippingMarkModel,
  ShippingMarkPrintingModel,
  ShippingMarkShippingModel,
  ShippingRequestClients,
  ShippingRequestModel,
} from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { EventType } from 'app/shared/enumerations/import-event-type.enum';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './shipping-mark.component.html',
  styleUrls: ['./shipping-mark.component.scss'],
})
export class ShippingMarkComponent implements OnInit, OnDestroy {
  title = 'Shipping Mark';
  titleDialog = '';

  shippingMarkForm: FormGroup;

  user: ApplicationUser;
  shippingMarks: ShippingMarkModel[] = [];
  selectedShippingMark: ShippingMarkModel;
  shippingRequests: ShippingRequestModel[] = [];

  currentShippingMark: ShippingMarkModel;
  currentShippingMarkShippingModel: ShippingMarkShippingModel;

  shippingMarkPrintings: ShippingMarkPrintingModel[] = [];
  shippingMarkShippings: ShippingMarkShippingModel[] = [];

  canRePrint = false;

  isEdit = false;
  isShowDialog = false;
  isShowDialogCreate = false;
  isShowDialogDetail = false;
  isShowDialogHistory = false;
  isShowDialogEdit = false;

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
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private shippingRequestClients: ShippingRequestClients,
    private authenticationService: AuthenticationService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));
    this.canRePrint = this.printService.canRePrint(this.user);

    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: '....', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initShippingRequests();
    this.initShippingMarks();
  }

  initForm() {
    this.shippingMarkForm = this.fb.group({
      id: [0],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      shippingRequest: [null, Validators.required],
      shippingMarkShippings: this.fb.array([]),
      receivedMarkPrintings: this.fb.array([]),
    });
  }

  initShippingRequests() {
    this.shippingRequestClients
      .getShippingRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingRequests = i),
        (_) => (this.shippingRequests = [])
      );
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

  openCreateDialog() {
    this.shippingMarkForm.reset();
    this.titleDialog = 'Create Shipping Mark';
    this.isShowDialogCreate = true;
    this.isEdit = false;
  }

  onCreate() {
    const model = this.shippingMarkForm.value as ShippingMarkModel;
    model.id = 0;

    this.shippingMarkClients
      .addShippingMark(model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Create Shipping Mark Successfully');
            this.initShippingMarks();
            this.hideDialog();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => {
          this.notificationService.error('Create Shipping Mark Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  onSubmit() {
    if (this.shippingMarkForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  openEditDialog(shippingMark: ShippingMarkModel) {
    this.titleDialog = 'Edit Shipping Mark';
    this.isEdit = true;

    this.shippingMarkClients
      .getShippingMarkById(shippingMark.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ShippingMarkModel) => {
          let shippingRequest = null;

          i.shippingMarkShippings.forEach((item) => {
            shippingRequest = item.shippingRequest;
            item['selectedReceivedMarks'] = item.product.receivedMarkPrintings.filter((p) => i.receivedMarkPrintings.some((r) => r.id === p.id));
          });

          this.currentShippingMark = i;
          this.isShowDialogEdit = true;

          this.shippingMarkForm.patchValue(i);
          this.shippingMarkForm.get('shippingRequest').patchValue(shippingRequest);
        },
        (_) => this.notificationService.error('Failed to open Edit Shipping Mark')
      );
  }

  hideDialog(eventType = EventType.HideDialog) {
    if (eventType == EventType.RefreshData && this.selectedShippingMark) {
      this.getShippingMarkMovementRequestsFullInfo(this.selectedShippingMark);
    }

    this.isShowDialog = false;
    this.isShowDialogCreate = false;
    this.isShowDialogHistory = false;
    this.isShowDialogDetail = false;
    this.isShowDialogEdit = false;
  }

  onEdit() {
    const { id } = this.shippingMarkForm.value;

    this.shippingMarkClients
      .updateShippingRequest(id, this.shippingMarkForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Edit Shipping Mark Successfully');
            this.initShippingMarks();
            this.hideDialog();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => {
          this.notificationService.error('Edit Shipping Mark Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  openDeleteDialog(shippingMark: ShippingMarkModel) {
    this.confirmationService.confirm({
      message: 'Do you confirm to delete this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingMarkClients
          .deleteShippingMarkAysnc(shippingMark.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.notificationService.success('Delete Shipping Mark Successfully');
                this.initShippingMarks();
                this.selectedShippingMark = null;
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => {
              this.notificationService.error('Delete Shipping Mark Failed. Please try again');
            }
          );
      },
    });
  }

  handleSelectedShippingRequest(shippingRequest: ShippingRequestModel) {
    if (shippingRequest) {
      this.shippingMarkClients
        .generateShippingMarkShippings(shippingRequest)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (shippingMarkShippings) => {
            this.shippingMarkShippings = shippingMarkShippings;
            this.shippingMarkShippings.forEach((item) => {
              item['selectedReceivedMarks'] = [];
            });
          },
          (_) => (this.shippingMarkShippings = [])
        );
    }
  }

  onPrint() {
    this.printService.printDocument('shipping-mark');
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
          shippingMark.shippingMarkShippings.forEach((item: ShippingMarkShippingModel) => {
            const shippingMarkShipping = shippingMarkShippings.find((i) => i.shippingMarkId === item.shippingMarkId && i.productId === item.productId);

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

  completeShippingRequest(shippingMark: ShippingMarkModel) {
    this.confirmationService.confirm({
      message: 'Do you confirm to complete shipping request of this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingRequestClients
          .completeShippingRequest(shippingMark.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.notificationService.success('Confirm Shipping Request Successfully');
                this.selectedShippingMark = null;
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => {
              this.notificationService.error('Confirm Shipping Request Failed. Please try again');
            }
          );
      },
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
