import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel, WorkOrderDetailModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-work-order-create',
  templateUrl: './work-order-create.component.html',
  styleUrls: ['./work-order-create.component.scss'],
})
export class WorkOrderCreateComponent implements OnInit {
  @Input() products: ProductModel[] = [];
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() workOrderForm: FormGroup;
  @Output() submitEvent = new EventEmitter<any>(null);
  @Output() hideDialogEvent = new EventEmitter<any>();

  selectedProducts: ProductModel[] = [];
  workOrderDetails: WorkOrderDetailModel[] = [];
  clonedWorkOrderDetailModels: { [s: string]: WorkOrderDetailModel } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  productCols: any[] = [];
  productFields: any[] = [];

  constructor(private fb: FormBuilder) {}

  get notesControl() {
    return this.workOrderForm.get('notes');
  }

  get workOrderDetailsControl() {
    return this.workOrderForm.get('workOrderDetails') as FormArray;
  }

  ngOnInit(): void {
    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Name', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.productFields = this.productCols.map((i) => i.field);

    this.stepItems = [{ label: 'Products' }, { label: 'Confirmation' }, { label: 'Complete' }];
  }

  hideDialog() {
    this.selectedProducts = [];
    this.workOrderDetails = [];
    this.stepIndex = 0;
    this.workOrderForm.reset();
    this.hideDialogEvent.emit();
  }

  onSubmit() {
    this.workOrderDetailsControl.clear();

    this.workOrderDetails.forEach((i) => {
      this.workOrderDetailsControl.push(this.initWorkOrderDetailForm(i));
    });

    this.submitEvent.emit();
  }

  initWorkOrderDetailForm(workOrderDetail: WorkOrderDetailModel) {
    return this.fb.group({
      quantity: [workOrderDetail.quantity],
      productId: [workOrderDetail.productId],
      workOrderId: [workOrderDetail.workOrderId],
    });
  }

  onRowEditInit(workOrderDetail: WorkOrderDetailModel) {
    this.clonedWorkOrderDetailModels[workOrderDetail.productId] = { ...workOrderDetail };
  }

  onRowDelete(workOrderDetail: WorkOrderDetailModel) {
    this.selectedProducts = this.selectedProducts.filter((i) => i.id !== workOrderDetail.productId);
    this.workOrderDetails = this.workOrderDetails.filter((i) => i.productId !== workOrderDetail.productId);
  }

  onRowEditSave(workOrderDetail: WorkOrderDetailModel) {
    const entity = this.workOrderDetails.find((i) => i.productId === workOrderDetail.productId);
    entity.quantity = workOrderDetail.quantity;
    delete this.clonedWorkOrderDetailModels[workOrderDetail.productId];
  }

  onRowEditCancel(workOrderDetail: WorkOrderDetailModel, index: number) {
    this.workOrderDetails[index] = this.clonedWorkOrderDetailModels[workOrderDetail.productId];
    delete this.clonedWorkOrderDetailModels[workOrderDetail.productId];
  }

  checkModifiedQuantity(workOrderDetails: WorkOrderDetailModel[]) {
    return workOrderDetails.filter((i) => i.quantity === 0).length === 0;
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
}
