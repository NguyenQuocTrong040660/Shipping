import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { WorkOrderDetailModel, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-work-order-edit',
  templateUrl: './work-order-edit.component.html',
  styleUrls: ['./work-order-edit.component.scss'],
})
export class WorkOrderEditComponent implements OnInit, OnChanges {
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() workOrderForm: FormGroup;
  @Input() workOrder: WorkOrderModel;
  @Output() submitEvent = new EventEmitter<any>(null);
  @Output() hideDialogEvent = new EventEmitter<any>();

  workOrderDetails: WorkOrderDetailModel[] = [];
  clonedWorkOrderDetailModels: { [s: string]: WorkOrderDetailModel } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  get notesControl() {
    return this.workOrderForm.get('notes');
  }

  get workOrderDetailsControl() {
    return this.workOrderForm.get('workOrderDetails') as FormArray;
  }

  constructor(private fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.workOrder && changes.workOrder.currentValue) {
      const workOrder = changes.workOrder.currentValue as WorkOrderModel;
      this.workOrderDetails = workOrder.workOrderDetails;
      this.workOrderForm.patchValue(workOrder);
    }
  }

  ngOnInit(): void {
    this.stepItems = [{ label: 'Work Order Details' }, { label: 'Complete' }];
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

  hideDialog() {
    this.workOrderDetails = [];
    this.stepIndex = 0;
    this.workOrderForm.reset();
    this.hideDialogEvent.emit();
  }

  checkModifiedQuantity(workOrderDetails: WorkOrderDetailModel[]) {
    return workOrderDetails.filter((i) => i.quantity === 0).length === 0;
  }

  onRowEditInit(workOrderDetail: WorkOrderDetailModel) {
    this.clonedWorkOrderDetailModels[workOrderDetail.productId] = { ...workOrderDetail };
  }

  onRowDelete(workOrderDetail: WorkOrderDetailModel) {
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

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        break;
      }
      case 1: {
        break;
      }
    }

    this.stepIndex += 1;
  }

  prevPage() {
    if (this.stepIndex === 0) return this.hideDialog();

    this.stepIndex -= 1;
  }
}
