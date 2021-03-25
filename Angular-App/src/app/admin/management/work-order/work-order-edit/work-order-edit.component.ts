import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { ProductModel } from 'app/shared/api-clients/communications.client';
import { WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { MenuItem } from 'primeng/api';
import { WorkOrderDetail } from '../work-order.component';

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

  workOrderDetails: WorkOrderDetail[] = [];
  clonedWorkOrderDetailModels: { [s: string]: WorkOrderDetail } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  TypeColumn = TypeColumn;

  get refIdControl() {
    return this.workOrderForm.get('refId');
  }

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
      this.workOrderDetails.forEach((w) => (w.isEditRow = false));
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

  initWorkOrderDetailForm(workOrderDetail: WorkOrderDetail) {
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

  allowMoveToCompleteStep(workOrderDetails: WorkOrderDetail[]): boolean {
    const haveFilledDataRows = workOrderDetails.filter((i) => i.quantity === 0).length === 0;
    const haveNotEditRows = workOrderDetails.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.workOrderDetails.length > 0;
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
    this.stepIndex -= 1;
  }
}
