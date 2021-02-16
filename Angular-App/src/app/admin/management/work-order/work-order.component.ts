import { ProductClients, ProductModel, WorkOrderClients, WorkOrderDetailModel, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService, MenuItem, SelectItem } from 'primeng/api';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';

@Component({
  templateUrl: './work-order.component.html',
  styleUrls: ['./work-order.component.scss'],
})
export class WorkOrderComponent implements OnInit {
  workOrders: WorkOrderModel[] = [];
  products: ProductModel[] = [];
  selectedWorkOrder: WorkOrderModel;
  selectItems: SelectItem[] = [];

  selectedProducts: ProductModel[] = [];
  workOrderDetails: WorkOrderDetailModel[] = [];
  clonedWorkOrderDetailModels: { [s: string]: WorkOrderDetailModel } = {};

  workOrderForm: FormGroup;

  stepItems: MenuItem[];
  stepIndex = 0;

  isEdit = false;
  isShowDialog = false;
  titleDialog = '';

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  cols: any[] = [];
  fields: any[] = [];

  productCols: any[] = [];
  productFields: any[] = [];

  get refIdControl() {
    return this.workOrderForm.get('refId');
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
      { header: 'RefId', field: 'refId', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Last Modified', field: 'lastModified', width: WidthColumn.NormalColumn, type: TypeColumn.DateColumn },
      { header: 'Last Modified By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Name', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.productFields = this.productCols.map((i) => i.field);

    this.stepItems = [{ label: 'Products' }, { label: 'Confirmation' }, { label: 'Complete' }];

    this.initWorkOrders();
    this.initProducts();
    this.initWorkOrderForm();
  }

  initWorkOrderForm() {
    this.workOrderForm = new FormGroup({
      id: new FormControl(0),
      refId: new FormControl('', Validators.required),
      notes: new FormControl(''),
      workOrderDetails: new FormArray([]),
      lastModifiedBy: new FormControl(''),
      lastModified: new FormControl(null),
    });
  }

  initWorkOrderDetailForm() {
    return new FormGroup({
      id: new FormControl(0),
      quantity: new FormControl(0, Validators.required),
      workOrderId: new FormControl(0, Validators.required),
      productId: new FormControl(0, Validators.required),
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
      };
    });
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
    this.selectedProducts = [];
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
          },
          (_) => this.notificationService.error('Delete Work Order Failed. Please try again')
        );
      },
    });
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.workOrderDetails = this._mapToProductsToWorkOrderDetails(this.selectedProducts);
        break;
      }
      case 1: {
        break;
      }
    }

    this.stepIndex += 1;
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  onRowEditInit(workOrderDetail: WorkOrderDetailModel) {
    this.clonedWorkOrderDetailModels[workOrderDetail.id] = { ...workOrderDetail };
  }

  onRowDelete(workOrderDetail: WorkOrderDetailModel) {
    this.selectedProducts = this.selectedProducts.filter((i) => i.id !== workOrderDetail.productId);
    this.workOrderDetails = this.workOrderDetails.filter((i) => i.id !== workOrderDetail.id);
  }

  onRowEditSave(workOrderDetail: WorkOrderDetailModel) {
    const entity = this.workOrderDetails.find((i) => i.id === workOrderDetail.id);
    entity.quantity = workOrderDetail.quantity;
    delete this.clonedWorkOrderDetailModels[workOrderDetail.id];
  }

  onRowEditCancel(workOrderDetail: WorkOrderDetailModel, index: number) {
    this.workOrderDetails[index] = this.clonedWorkOrderDetailModels[workOrderDetail.id];
    delete this.clonedWorkOrderDetailModels[workOrderDetail.id];
  }
}
