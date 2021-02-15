import { ProductClients, ProductModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NotificationService } from 'app/shared/services/notification.service';
import { SelectItem } from 'primeng/api';

@Component({
  templateUrl: './work-order.component.html',
  styleUrls: ['./work-order.component.scss'],
})
export class WorkOrderComponent implements OnInit {
  workOrders: WorkOrderModel[] = [];
  products: ProductModel[] = [];
  selectedWorkOrders: WorkOrderModel[] = [];
  selectItems: SelectItem[] = [];

  isShowDeleteDialog: boolean;
  currentSelectedWorkOrder: WorkOrderModel[] = [];
  isDeleteMany: boolean;
  workOrderForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowHistoryDialog = false;
  titleDialog = '';

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

  constructor(private workOrderClients: WorkOrderClients, private productClients: ProductClients, private notificationService: NotificationService) {}

  ngOnInit() {
    this.cols = [
      { header: 'ID', field: 'id' },
      { header: 'Product Number', field: 'product', subField: 'productNumber' },
      { header: 'Qty', field: 'quantity' },
      { header: 'Moving Qty', field: 'movingQuantity' },
      { header: 'Remain Qty', field: 'remainQuantity' },
      { header: 'Notes', field: 'notes' },
      { header: 'Created', field: 'created', isDate: true },
      { header: 'Create By', field: 'createBy' },
      { header: 'Last Modified', field: 'lastModified', isDate: true },
      { header: 'Last Modified By', field: 'lastModifiedBy' },
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

        console.log(i);
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

  // Delete Work Order
  openDeleteDialog(singleWorkOrder?: WorkOrderModel) {
    this.isShowDeleteDialog = true;
    this.currentSelectedWorkOrder = [];

    if (singleWorkOrder) {
      this.isDeleteMany = false;
      this.currentSelectedWorkOrder.push(singleWorkOrder);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.currentSelectedWorkOrder.length === 0) {
      return;
    }

    if (this.isDeleteMany) {
      console.log('this.selectedWorkOrders: ' + this.selectedWorkOrders);
    } else {
      const workOrder = this.currentSelectedWorkOrder[0];
      this.workOrderClients.deleteWorkOrderAysnc(workOrder.id).subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Delete Work Order Successfully');
            this.initWorkOrders();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDeleteDialog();
        },
        (_) => {
          this.notificationService.error('Delete Work Order Failed. Please try again');
          this.hideDialog();
        }
      );
    }
  }
}
