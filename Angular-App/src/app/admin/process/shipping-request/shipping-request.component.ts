import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ProductClients, ProductModel, ShippingPlanClients, ShippingPlanModel, ShippingRequestClients, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enums/history-dialog-type.enum';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService, MenuItem, SelectItem } from 'primeng/api';

@Component({
  templateUrl: './shipping-request.component.html',
  styleUrls: ['./shipping-request.component.scss'],
})
export class ShippingRequestComponent implements OnInit {
  shippingRequests: ShippingRequestModel[] = [];
  shippingPlans: ShippingPlanModel[] = [];
  selectedShippingRequest: ShippingRequestModel;
  selectedShippingPlan: ShippingPlanModel;

  selectItems: SelectItem[] = [];
  products: ProductModel[] = [];

  shippingRequestForm: FormGroup;

  isEdit = false;
  isShowDialogCreate = false;
  isShowDialogEdit = false;
  isShowDialogHistory = false;
  HistoryDialogType = HistoryDialogType;
  titleDialog = '';

  stepItems: MenuItem[] = [];
  cols: any[] = [];
  colShippingPlans: any[] = [];
  fields: any[] = [];
  fieldShippingPlans: any[] = [];

  activeIndex = 0;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  get purchaseOrderControl() {
    return this.shippingRequestForm.get('purchaseOrder');
  }

  get semlineNumberControl() {
    return this.shippingRequestForm.get('semlineNumber');
  }

  get customerNameControl() {
    return this.shippingRequestForm.get('customerName');
  }

  get salesPriceControl() {
    return this.shippingRequestForm.get('salesPrice');
  }

  get salesIdControl() {
    return this.shippingRequestForm.get('salesID');
  }

  get quantityControl() {
    return this.shippingRequestForm.get('quantityOrder');
  }

  get shippingDateControl() {
    return this.shippingRequestForm.get('shippingDate');
  }

  get shippingModeControl() {
    return this.shippingRequestForm.get('shippingMode');
  }

  get productControl() {
    return this.shippingRequestForm.get('productId');
  }

  get notesControl() {
    return this.shippingRequestForm.get('notes');
  }

  constructor(
    private shippingRequestClients: ShippingRequestClients,
    private shippingPlanClients: ShippingPlanClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService
  ) { }

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'product', subField: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Purchase Order', field: 'purchaseOrder', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Order', field: 'quantityOrder', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Customer Name', field: 'customerName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Sales Price', field: 'salesPrice', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Sales ID', field: 'salesID', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Semline Number', field: 'semlineNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Shipping Mode', field: 'shippingMode', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Shipping Date', field: 'shippingDate', width: WidthColumn.NormalColumn, type: TypeColumn.DateColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn }
    ];

    this.fields = this.cols.map((i) => i.field);
    this.colShippingPlans = this.cols.filter((i) => i.field !== 'created' && i.field !== 'createBy' && i.field !== 'lastModified' && i.field !== 'lastModifiedBy');
    this.fieldShippingPlans = this.colShippingPlans.map((i) => i.field);

    this.stepItems = [{ label: 'Shipping Plan' }, { label: 'Confirmation' }];

    this.initForm();
    this.initShippingPlan();
    this.initDataSource();
    this.initProducts();
  }

  initShippingPlan() {
    this.shippingPlanClients.getAllShippingPlan().subscribe(
      (i) => (this.shippingPlans = i),
      (_) => (this.shippingPlans = [])
    );
  }

  initForm() {
    this.shippingRequestForm = new FormGroup({
      id: new FormControl(0),
      purchaseOrder: new FormControl('', Validators.required),
      customerName: new FormControl('', Validators.required),
      quantityOrder: new FormControl(0, Validators.required),
      salesPrice: new FormControl(0, Validators.required),
      salesID: new FormControl(0, Validators.required),
      semlineNumber: new FormControl(0, Validators.required),
      shippingMode: new FormControl('', Validators.required),
      shippingDate: new FormControl(null, Validators.required),
      notes: new FormControl(''),
      productId: new FormControl(0, Validators.required),
      created: new FormControl(null),
      createBy: new FormControl(''),
      lastModified: new FormControl(null),
      lastModifiedBy: new FormControl(''),
    });
  }

  initDataSource() {
    this.shippingRequestClients.getShippingRequests().subscribe(
      (i) => (this.shippingRequests = i),
      (_) => (this.shippingRequests = [])
    );
  }

  initProducts() {
    this.productClients.getProducts().subscribe(
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

  // Create Shipping Request
  openCreateDialog() {
    this.shippingRequestForm.reset();
    this.titleDialog = 'Create Shipping Request';
    this.isShowDialogCreate = true;
    this.isEdit = false;
  }

  onCreate() {
    const model = this.shippingRequestForm.value as ShippingRequestModel;
    model.id = 0;

    this.shippingRequestClients.addShippingRequest(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Shipping Request Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialogCreate();
      },
      (_) => {
        this.notificationService.error('Create Shipping Request Failed. Please try again');
        this.hideDialogCreate();
      }
    );
  }

  onSubmit() {
    if (this.shippingRequestForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  // Edit Shipping Mark
  openEditDialog(shippingRequest: ShippingRequestModel) {
    this.isShowDialogEdit = true;
    this.titleDialog = 'Create Shipping Request';
    this.isEdit = true;
    this.shippingRequestForm.patchValue(shippingRequest);
  }

  hideDialogEdit() {
    this.isShowDialogEdit = false;
  }

  hideDialogCreate() {
    this.activeIndex = 0;
    this.selectedShippingPlan = null;
    this.isShowDialogCreate = false;
  }

  onEdit() {
    const { id } = this.shippingRequestForm.value;

    this.shippingRequestClients.updateShippingRequest(id, this.shippingRequestForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Shipping Request Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialogEdit();
      },
      (_) => {
        this.notificationService.error('Edit Shipping Request Failed. Please try again');
        this.hideDialogEdit();
      }
    );
  }

  openDeleteDialog(singleShippingRequest: ShippingRequestModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingRequestClients.deleteShippingRequestAysnc(singleShippingRequest.id).subscribe(
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

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        if (!this.selectedShippingPlan) {
          this.confirmationService.confirm({
            message: 'Are you sure you want to create shipping request without selecting shipping plan?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => (this.activeIndex += 1),
          });

          return;
        }

        const shippingPlan = this.shippingPlans.find((i) => i.id === this.selectedShippingPlan.id);
        shippingPlan.shippingDate = new Date(shippingPlan.shippingDate);
        this.shippingRequestForm.patchValue(shippingPlan);
        this.activeIndex += 1;
        break;
      }
    }
  }

  prevPage() {
    this.activeIndex -= 1;
  }

  getDetailShippingRequest(shippingRequest: ShippingRequestModel) {
    // TODO: show Shipping Request Detail
  }

  hideDialog() {
    this.isShowDialogHistory = false;
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }
}
