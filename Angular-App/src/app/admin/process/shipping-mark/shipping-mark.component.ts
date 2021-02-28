import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ProductClients, ProductModel, ShippingMarkClients, ShippingMarkModel, ShippingRequestClients, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './shipping-mark.component.html',
  styleUrls: ['./shipping-mark.component.scss'],
})
export class ShippingMarkComponent implements OnInit, OnDestroy {
  title = 'Shipping Mark';

  user: ApplicationUser;
  shippingMarks: ShippingMarkModel[] = [];
  products: ProductModel[] = [];
  selectedShippingMark: ShippingMarkModel;
  selectItems: SelectItem[] = [];
  shippingRequests: ShippingRequestModel[] = [];

  isShowDeleteDialog: boolean;
  currentSelectedShippingMark: ShippingMarkModel[] = [];
  isDeleteMany: boolean;
  shippingMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogHistory = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  rowGroupMetadata: any;

  get quantityControl() {
    return this.shippingMarkForm.get('quantity');
  }

  get productControl() {
    return this.shippingMarkForm.get('productId');
  }

  get notesControl() {
    return this.shippingMarkForm.get('notes');
  }

  get customerControl() {
    return this.shippingMarkForm.get('customerId');
  }

  get revisionControl() {
    return this.shippingMarkForm.get('revision');
  }

  get cartonNumberControl() {
    return this.shippingMarkForm.get('cartonNumber');
  }

  private destroyed$ = new Subject<void>();

  constructor(
    public printService: PrintService,
    private shippingMarkClients: ShippingMarkClients,
    private productClients: ProductClients,
    private notificationService: NotificationService,
    private shippingRequestClients: ShippingRequestClients,
    private confirmationService: ConfirmationService,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Sequence', field: 'sequence', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Quantity', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Revision', field: 'revision', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Print By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Print Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initProducts();
    this.intMovementRequest();
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));
  }

  initForm() {
    this.shippingMarkForm = new FormGroup({
      id: new FormControl(0),
      productId: new FormControl(0, Validators.required),
      notes: new FormControl(''),
      revision: new FormControl('', [Validators.required]),
      quantity: new FormControl(0, [Validators.required, Validators.min(1)]),
      cartonNumber: new FormControl('', [Validators.required]),
      customerId: new FormControl('', [Validators.required]),
      lastModifiedBy: new FormControl(''),
      lastModified: new FormControl(null),
    });
  }

  intMovementRequest() {
    this.shippingRequestClients
      .getShippingRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.shippingRequests = i;
        },
        (_) => (this.shippingRequests = [])
      );
  }

  initProducts() {
    this.productClients
      .getProducts()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.products = i;
        },
        (_) => (this.products = [])
      );
  }

  initShippingMarks(shippingRequestId: number) {
    this.shippingMarkClients
      .getShippingMarksByShippingRequestId(shippingRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.shippingMarks = i;
          this.updateRowGroupMetaData();
        },
        (_) => (this.shippingMarks = [])
      );
  }

  handleOnChange(selectedShippingRequestId) {
    this.initShippingMarks(selectedShippingRequestId);
  }

  initDataSource() {
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
    this.isShowDialog = true;
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
            this.initDataSource();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
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
    this.isShowDialog = true;
    this.titleDialog = 'Create Shipping Mark';
    this.isEdit = true;
    this.shippingMarkForm.patchValue(shippingMark);
  }

  hideDialog() {
    this.isShowDialog = false;
    this.isShowDialogHistory = false;
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
            this.initDataSource();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Edit Shipping Mark Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  openDeleteDialog(shippingMark: ShippingMarkModel) {
    this.shippingMarkClients
      .deleteShippingMarkAysnc(shippingMark.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Delete Shipping Mark Successfully');
            this.initDataSource();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => {
          this.notificationService.error('Delete Shipping Mark Failed. Please try again');
        }
      );
  }

  onPrint() {
    this.printService.printDocument('shipping-mark');
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  onSort() {
    this.updateRowGroupMetaData();
  }

  customSort() {
    this.shippingMarks.sort((a, b) => {
      const aGroup = a.product.productNumber.toLowerCase();
      const bGroup = b.product.productNumber.toLowerCase();

      if (aGroup > bGroup) {
        return 1;
      }
      if (aGroup < bGroup) {
        return -1;
      }

      const aSequence = a.sequence;
      const bSequene = b.sequence;

      if (aSequence > bSequene) {
        return 1;
      }
      if (aSequence < bSequene) {
        return -1;
      }
      return 0;
    });
  }

  updateRowGroupMetaData() {
    this.rowGroupMetadata = {};

    if (this.shippingMarks) {
      for (let i = 0; i < this.shippingMarks.length; i++) {
        let rowData = this.shippingMarks[i];
        let representativeName = rowData.product.productNumber;

        if (i == 0) {
          this.rowGroupMetadata[representativeName] = { index: 0, size: 1 };
        } else {
          let previousRowData = this.shippingMarks[i - 1];
          let previousRowGroup = previousRowData.product.productNumber;
          if (representativeName === previousRowGroup) this.rowGroupMetadata[representativeName].size++;
          else this.rowGroupMetadata[representativeName] = { index: i, size: 1 };
        }
      }
    }
  }

  _mapToShippingRequestSelectItems(shippingRequests: ShippingRequestModel[]): SelectItem[] {
    return shippingRequests.map((i) => ({
      value: i.id,
      label: `${i.identifier}`,
    }));
  }

  printShippingMark(shippingMark: ShippingMarkModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to print mark for this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingMarkClients
          .printShippingMark(shippingMark.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.onPrint();
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => {
              this.notificationService.error('Print Shipping Mark Failed. Please try again');
            }
          );
      },
    });
  }

  getHelpText() {
    return this.printService.canRePrint(this.user) ? '' : 'Shipping Mark has been printed. Please contact your manager to re-print';
  }

  canRePrint() {
    return this.printService.canRePrint(this.user) ? true : false;
  }

  handleRePrintMark(item: ShippingMarkModel) {
    if (!this.canRePrint()) {
      return;
    }

    this.confirmationService.confirm({
      message: 'Are you sure you want to re-print this mark ?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingMarkClients
          .printShippingMark(item.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.onPrint();
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => this.notificationService.error('Print Shipping Mark Failed. Please try again')
          );
      },
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
