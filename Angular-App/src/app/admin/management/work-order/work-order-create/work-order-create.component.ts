import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ProductModel } from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';
import { WorkOrder } from '../work-order.component';

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

  selectedProduct: ProductModel;
  workOrders: WorkOrder[] = [];
  clonedWorkOrders: { [s: string]: WorkOrder } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  productCols: any[] = [];
  productFields: any[] = [];

  constructor() {}

  get notesControl() {
    return this.workOrderForm.get('notes');
  }

  get refIdControl() {
    return this.workOrderForm.get('refId');
  }

  get quantityControl() {
    return this.workOrderForm.get('quantity');
  }

  get productIdControl() {
    return this.workOrderForm.get('productId');
  }

  ngOnInit(): void {
    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Description', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.productFields = this.productCols.map((i) => i.field);

    this.stepItems = [{ label: 'Products' }, { label: 'Work Order Details' }, { label: 'Complete' }];
  }

  hideDialog() {
    this.selectedProduct = null;
    this.workOrders = [];
    this.stepIndex = 0;

    this.workOrderForm.reset();
    this.hideDialogEvent.emit();
  }

  onSubmit() {
    this.submitEvent.emit();
  }

  onRowEditInit(workOrder: WorkOrder) {
    workOrder.isEditRow = true;
    this.clonedWorkOrders[workOrder.productId] = { ...workOrder };
  }

  onRowEditSave(workOrder: WorkOrder) {
    workOrder.isEditRow = false;
    const entity = this.workOrders.find((i) => i.productId === workOrder.productId);
    entity.quantity = workOrder.quantity;
    delete this.clonedWorkOrders[workOrder.productId];
  }

  onRowEditCancel(workOrder: WorkOrder, index: number) {
    this.clonedWorkOrders[index] = this.clonedWorkOrders[workOrder.productId];
    this.workOrders[index].isEditRow = false;
    delete this.clonedWorkOrders[workOrder.productId];
  }

  allowMoveToCompleteStep(workOrders: WorkOrder[]): boolean {
    const haveFilledDataRows = workOrders.filter((i) => i.quantity === 0).length === 0;
    const haveNotEditRows = workOrders.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.workOrders.length > 0;
  }

  _mapToProductsToWorkOrders(products: ProductModel[]): WorkOrder[] {
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
        this.workOrders = this._mapToProductsToWorkOrders([this.selectedProduct]);
        break;
      }
      case 1: {
        const workOrder = this.workOrders && this.workOrders.length && this.workOrders[0];

        if (workOrder) {
          this.productIdControl.setValue(workOrder.productId);
          this.quantityControl.setValue(workOrder.quantity);
        }
        break;
      }
    }

    this.stepIndex += 1;
  }

  prevPage() {
    this.stepIndex -= 1;
  }
}
