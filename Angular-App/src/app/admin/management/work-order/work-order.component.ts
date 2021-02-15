import { ProductClients, ProductModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';

@Component({
  templateUrl: './work-order.component.html',
  styleUrls: ['./work-order.component.scss'],
})
export class WorkOrderComponent implements OnInit {
  workOrders: WorkOrderModel[] = [];
  products: ProductModel[] = [];
  selectedWorkOrders: WorkOrderModel[] = [];
  selectItems: SelectItem[] = [];

  workOrderForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  titleDialog = '';

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  cols: any[] = [];
  fields: any[] = [];

  get quantityControl() {
    return this.workOrderForm.get('quantity');
  }

  get movingQuantityControl() {
    return this.workOrderForm.get('movingQuantity');
  }

  get remainQuantityControl() {
    return this.workOrderForm.get('remainQuantity');
  }

  get productControl() {
    return this.workOrderForm.get('productId');
  }

  get notesControl() {
    return this.workOrderForm.get('notes');
  }

  constructor(
    private workOrderClients: WorkOrderClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'ID', field: 'id', width: WidthColumn.IdentityColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Number', field: 'product', subField: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Qty', field: 'quantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Moving Qty', field: 'movingQuantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Remain Qty', field: 'remainQuantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Created', field: 'created', width: WidthColumn.NormalColumn, type: TypeColumn.DateColumn },
      { header: 'Create By', field: 'createBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Last Modified', field: 'lastModified', width: WidthColumn.NormalColumn, type: TypeColumn.DateColumn },
      { header: 'Last Modified By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initWorkOrders();
    this.initProducts();
    this.initForm();
  }

  initForm() {
    this.workOrderForm = new FormGroup({
      id: new FormControl(0),
      productId: new FormControl(0, Validators.required),
      quantity: new FormControl(0, [Validators.required, Validators.min(1)]),
      movingQuantity: new FormControl(0, [Validators.required, Validators.min(1)]),
      notes: new FormControl(''),
      remainQuantity: new FormControl(0, [Validators.required, Validators.min(1)]),
      movementRequestId: new FormControl(0),
      createdBy: new FormControl(''),
      created: new FormControl(null),
      lastModifiedBy: new FormControl(''),
      lastModified: new FormControl(null),
    });
  }

  initWorkOrders() {
    this.workOrderClients.getWorkOrders().subscribe(
      (i) => (this.workOrders = i),
      (_) => (this.workOrders = [])
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

  // Create Work Order
  openCreateDialog() {
    this.workOrderForm.reset();
    this.titleDialog = 'Create Work Order';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  hideDialog() {
    this.isShowDialog = false;
  }

  onSubmit() {
    if (this.workOrderForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  onCreate() {
    const model = this.workOrderForm.value as WorkOrderModel;
    model.id = 0;

    this.workOrderClients.addWorkOrder(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Work Order Successfully');
          this.initWorkOrders();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Work Order Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  // Edit Work Order
  openEditDialog(workOrder: WorkOrderModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Create Work Order';
    this.isEdit = true;
    this.workOrderForm.patchValue(workOrder);
  }

  onEdit() {
    const { id } = this.workOrderForm.value;

    this.workOrderClients.updateWorkOrder(id, this.workOrderForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Work Order Successfully');
          this.initWorkOrders();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Edit Work Order Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  openDeleteDialog(singleWorkOrder: WorkOrderModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.workOrderClients.deleteWorkOrderAysnc(singleWorkOrder.id).subscribe(
          (result) => {
            if (result && result.succeeded) {
              this.notificationService.success('Delete Work Order Successfully');
              this.initWorkOrders();
            } else {
              this.notificationService.error(result?.error);
            }

            this.selectedWorkOrders = this.selectedWorkOrders.filter((i) => i.id !== singleWorkOrder.id);
          },
          (_) => this.notificationService.error('Delete Work Order Failed. Please try again')
        );
      },
    });
  }
}
