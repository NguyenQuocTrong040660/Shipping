import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel } from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';
import { WorkOrderDetail } from '../work-order.component';

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
  workOrderDetails: WorkOrderDetail[] = [];
  clonedWorkOrderDetailModels: { [s: string]: WorkOrderDetail } = {};

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

  get refIdControl() {
    return this.workOrderForm.get('refId');
  }

  get workOrderDetailsControl() {
    return this.workOrderForm.get('workOrderDetails') as FormArray;
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

  initWorkOrderDetailForm(workOrderDetail: WorkOrderDetail) {
    return this.fb.group({
      quantity: [workOrderDetail.quantity],
      productId: [workOrderDetail.productId],
      workOrderId: [workOrderDetail.workOrderId],
    });
  }

  onRowEditInit(workOrderDetail: WorkOrderDetail) {
    workOrderDetail.isEditRow = true;
    this.clonedWorkOrderDetailModels[workOrderDetail.productId] = { ...workOrderDetail };
  }

  onRowEditSave(workOrderDetail: WorkOrderDetail) {
    workOrderDetail.isEditRow = false;
    const entity = this.workOrderDetails.find((i) => i.productId === workOrderDetail.productId);
    entity.quantity = workOrderDetail.quantity;
    delete this.clonedWorkOrderDetailModels[workOrderDetail.productId];
  }

  onRowEditCancel(workOrderDetail: WorkOrderDetail, index: number) {
    this.workOrderDetails[index] = this.clonedWorkOrderDetailModels[workOrderDetail.productId];
    this.workOrderDetails[index].isEditRow = false;
    delete this.clonedWorkOrderDetailModels[workOrderDetail.productId];
  }

  allowMoveToCompleteStep(workOrderDetails: WorkOrderDetail[]): boolean {
    const haveFilledDataRows = workOrderDetails.filter((i) => i.quantity === 0).length === 0;
    const haveNotEditRows = workOrderDetails.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.workOrderDetails.length > 0;
  }

  _mapToProductsToWorkOrderDetails(products: ProductModel[]): WorkOrderDetail[] {
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
        this.workOrderDetails = this._mapToProductsToWorkOrderDetails([this.selectedProduct]);
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
