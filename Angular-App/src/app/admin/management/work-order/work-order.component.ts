import { ProductClients, ProductModel, WorkOrderClients, WorkOrderDetailModel, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService } from 'primeng/api';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';

@Component({
  selector: 'app-work-order',
  templateUrl: './work-order.component.html',
  styleUrls: ['./work-order.component.scss'],
})
export class WorkOrderComponent implements OnInit {
  workOrders: WorkOrderModel[] = [];
  products: ProductModel[] = [];
  selectedWorkOrder: WorkOrderModel;

  workOrderForm: FormGroup;

  isEdit = false;
  isShowDialogCreate = false;
  isShowDialogEdit = false;
  titleDialog = '';

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  cols: any[] = [];
  fields: any[] = [];

  productCols: any[] = [];
  productFields: any[] = [];

  title = 'Work Orders Management';

  constructor(
    private workOrderClients: WorkOrderClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'WO-ID', field: 'id', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Reference ID', field: 'refId', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.NormalColumn, type: TypeColumn.DateColumn },
      { header: '', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initWorkOrders();
    this.initProducts();
    this.initWorkOrderForm();

    this.title = this.title.toUpperCase();
  }

  initWorkOrderForm() {
    this.workOrderForm = this.fb.group({
      id: [0],
      refId: [''],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      workOrderDetails: this.fb.array([]),
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
      (i) => (this.products = i),
      (_) => (this.products = [])
    );
  }

  _mapToProductsToWorkOrderDetails(products: ProductModel[]): WorkOrderDetailModel[] {
    return products.map((item, index) => {
      return {
        id: index + 1,
        productId: item.id,
        product: item,
        workOrder: null,
        workOrderId: 0,
        quantity: 0,
      };
    });
  }

  getDetailWorkOrder(workOrder: WorkOrderModel) {
    const workOrderSelected = this.workOrders.find((i) => i.id === workOrder.id);

    if (workOrderSelected && workOrderSelected.workOrderDetails && workOrderSelected.workOrderDetails.length > 0) {
      return;
    }

    this.workOrderClients.getWorkOrderById(workOrder.id).subscribe(
      (i: WorkOrderModel) => {
        workOrderSelected.workOrderDetails = i.workOrderDetails;
      },
      (_) => (workOrderSelected.workOrderDetails = [])
    );
  }

  // Create Work Order
  openCreateDialog() {
    this.titleDialog = 'Create Work Order';
    this.isShowDialogCreate = true;
    this.isEdit = false;
  }

  hideDialog() {
    this.isShowDialogCreate = false;
    this.isShowDialogEdit = false;
    this.workOrderForm.reset();
  }

  onSubmit() {
    if (this.workOrderForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  onCreate() {
    const model = this.workOrderForm.value as WorkOrderModel;

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
    this.isShowDialogEdit = true;
    this.titleDialog = 'Edit Work Order';
    this.isEdit = true;

    this.workOrderClients.getWorkOrderById(workOrder.id).subscribe(
      (i: WorkOrderModel) => {
        this.selectedWorkOrder.workOrderDetails = i.workOrderDetails;
      },
      (_) => (this.selectedWorkOrder.workOrderDetails = [])
    );
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
          },
          (_) => this.notificationService.error('Delete Work Order Failed. Please try again')
        );
      },
    });
  }
}
