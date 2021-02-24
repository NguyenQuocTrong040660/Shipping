import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ProductModel, WorkOrderDetailModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-shipping-plan-create',
  templateUrl: './shipping-plan-create.component.html',
  styleUrls: ['./shipping-plan-create.component.scss']
})
export class ShippingPlanCreateComponent implements OnInit {
  @Input() shippingPlanForm: FormGroup;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;
  @Input() products: ProductModel[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  stepItems: MenuItem[];
  stepIndex = 0;

  productCols: any[] = [];
  productFields: any[] = [];
  selectedProducts: ProductModel[] = [];

  workOrderDetails: WorkOrderDetailWithPriceModel[] = [];
  clonedWorkOrderDetailModels: { [s: string]: WorkOrderDetailWithPriceModel } = {};

  TypeColumn = TypeColumn;

  get customerNameControl() {
    return this.shippingPlanForm.get('customerName');
  }

  get semlineNumberControl() {
    return this.shippingPlanForm.get('semlineNumber');
  }

  get shippingModeControl() {
    return this.shippingPlanForm.get('shippingMode');
  }

  get shippingDateControl() {
    return this.shippingPlanForm.get('shippingDate');
  }

  get salesIdControl() {
    return this.shippingPlanForm.get('salesID');
  }

  get notesControl() {
    return this.shippingPlanForm.get('notes');
  }

  constructor() { }

  ngOnInit(): void {
    this.stepItems = [{ label: 'Shipping Plan Info' }, { label: 'Products' }, { label: 'Price' }, { label: 'Complete' }];

    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Name', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];
  }

  hideDialog() {
    this.hideDialogEvent.emit();
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 1: {
        this.workOrderDetails = this._mapToProductsToWorkOrderDetails(this.selectedProducts);
        break;
      }
    }
    this.stepIndex += 1;
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  onRowEditInit(workOrderDetail: WorkOrderDetailWithPriceModel) {
    this.clonedWorkOrderDetailModels[workOrderDetail.productId] = { ...workOrderDetail };
  }

  onRowDelete(workOrderDetail: WorkOrderDetailWithPriceModel) {
    this.selectedProducts = this.selectedProducts.filter((i) => i.id !== workOrderDetail.productId);
    this.workOrderDetails = this.workOrderDetails.filter((i) => i.productId !== workOrderDetail.productId);
  }

  onRowEditSave(workOrderDetail: WorkOrderDetailWithPriceModel) {
    const entity = this.workOrderDetails.find((i) => i.productId === workOrderDetail.productId);
    entity.quantity = workOrderDetail.quantity;
    entity.amount = workOrderDetail.quantity * workOrderDetail.price;
    delete this.clonedWorkOrderDetailModels[workOrderDetail.productId];
  }

  onRowEditCancel(workOrderDetail: WorkOrderDetailWithPriceModel, index: number) {
    this.workOrderDetails[index] = this.clonedWorkOrderDetailModels[workOrderDetail.productId];
    delete this.clonedWorkOrderDetailModels[workOrderDetail.productId];
  }

  checkModifiedQuantity(workOrderDetails: WorkOrderDetailWithPriceModel[]) {
    return workOrderDetails.filter((i) => i.quantity === 0).length === 0;
  }

  onSubmit() {
    this.submitEvent.emit();
  }

  _mapToProductsToWorkOrderDetails(products: ProductModel[]): WorkOrderDetailWithPriceModel[] {
    return products.map((item, index) => {
      return {
        id: index + 1,
        productId: item.id,
        product: item,
        workOrder: null,
        workOrderId: 0,
        quantity: 0,
        price: 0,
        amount: 0
      };
    });
  }
}

export interface WorkOrderDetailWithPriceModel extends WorkOrderDetailModel {
  price?: number;
  amount?: number;
}
