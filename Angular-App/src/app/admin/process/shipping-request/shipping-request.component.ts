import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductClients, ProductModel, ShippingPlanClients, ShippingPlanModel, ShippingRequestClients, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './shipping-request.component.html',
  styleUrls: ['./shipping-request.component.scss'],
})
export class ShippingRequestComponent implements OnInit, OnDestroy {
  title = 'Shipping Request Management';

  shippingRequests: ShippingRequestModel[] = [];
  shippingPlans: ShippingPlanModel[] = [];
  selectedShippingRequest: ShippingRequestModel;
  selectedShippingPlan: ShippingPlanModel;

  selectItems: SelectItem[] = [];
  products: ProductModel[] = [];

  shippingRequestForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogHistory = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];

  expandedItems: any[] = [];

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  private destroyed$ = new Subject<void>();

  constructor(
    private shippingRequestClients: ShippingRequestClients,
    private shippingPlanClients: ShippingPlanClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Purchase Order', field: 'purchaseOrder', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Customer Name', field: 'customerName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Sales Id', field: 'salesID', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Semline Number', field: 'semlineNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Shipping Date', field: 'shippingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: 'actions', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initShippingPlan();
    this.initDataSource();
    this.initProducts();
  }

  initShippingPlan() {
    this.expandedItems = [];

    this.shippingPlanClients
      .getAllShippingPlan()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingPlans = i),
        (_) => (this.shippingPlans = [])
      );
  }

  initForm() {
    this.shippingRequestForm = this.fb.group({
      id: [0],
      customerName: ['', [Validators.required]],
      salesID: [0, [Validators.required]],
      semlineNumber: [0, [Validators.required]],
      purchaseOrder: ['', [Validators.required]],
      shippingDate: ['', [Validators.required]],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      shippingRequestDetails: this.fb.array([]),
    });
  }

  initDataSource() {
    this.shippingRequestClients
      .getShippingRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingRequests = i),
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
          this.selectItems = this._mapToSelectItem(i);
        },
        (_) => (this.products = [])
      );
  }

  _mapToSelectItem(products: ProductModel[]): SelectItem[] {
    return products.map((p) => ({
      value: p.id,
      label: `${p.productNumber}-${p.productName}`,
    }));
  }

  handleSelectedShippingPlanEvent(shippingPlanId) {
    if (shippingPlanId) {
      this.shippingPlanClients
        .getShippingPlanById(shippingPlanId)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (shippingPlan) => (this.selectedShippingPlan = shippingPlan),
          (_) => this.notificationService.error('Failed to retrieve Shipping Plan')
        );
    }

    this.selectedShippingPlan = null;
  }

  openCreateDialog() {
    this.shippingRequestForm.reset();
    this.titleDialog = 'Create Shipping Request';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  onCreate() {
    const model = this.shippingRequestForm.value as ShippingRequestModel;
    model.id = 0;

    this.shippingRequestClients
      .addShippingRequest(model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Create Shipping Request Successfully');
            this.initDataSource();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Create Shipping Request Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  onSubmit() {
    if (this.shippingRequestForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  openEditDialog(shippingRequest: ShippingRequestModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Create Shipping Request';
    this.isEdit = true;
    this.shippingRequestForm.patchValue(shippingRequest);
  }

  hideDialog() {
    this.isShowDialog = false;
    this.isShowDialogHistory = false;
    this.isShowDialog = false;
    this.shippingRequestForm.reset();
  }

  onEdit() {
    const { id } = this.shippingRequestForm.value;

    this.shippingRequestClients
      .updateShippingRequest(id, this.shippingRequestForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Edit Shipping Request Successfully');
            this.initDataSource();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Edit Shipping Request Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  openDeleteDialog(singleShippingRequest: ShippingRequestModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingRequestClients
          .deleteShippingRequestAysnc(singleShippingRequest.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.notificationService.success('Delete Shipping Request Successfully');
                this.initDataSource();
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => this.notificationService.error('Delete Shipping Request Failed. Please try again')
          );
      },
    });
  }

  getDetailShippingRequest(shippingRequest: ShippingRequestModel) {
    const shippingRequestSelected = this.shippingRequests.find((i) => i.id === shippingRequest.id);

    if (shippingRequestSelected && shippingRequestSelected.shippingRequestDetails && shippingRequestSelected.shippingRequestDetails.length > 0) {
      return;
    }

    this.shippingRequestClients
      .getShippingRequestById(shippingRequest.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ShippingRequestModel) => (shippingRequestSelected.shippingRequestDetails = i.shippingRequestDetails),
        (_) => (shippingRequestSelected.shippingRequestDetails = [])
      );
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
