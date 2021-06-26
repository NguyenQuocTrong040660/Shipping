import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { WorkOrderModel } from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { MenuItem } from 'primeng/api';
import { WorkOrder } from '../work-order.component';

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

  workOrders: WorkOrder[] = [];
  clonedWorkOrderModels: { [s: string]: WorkOrder } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  TypeColumn = TypeColumn;

  get refIdControl() {
    return this.workOrderForm.get('refId');
  }

  get notesControl() {
    return this.workOrderForm.get('notes');
  }

  get quantityControl() {
    return this.workOrderForm.get('quantity');
  }

  get productIdControl() {
    return this.workOrderForm.get('productId');
  }

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.workOrder && changes.workOrder.currentValue) {
      const workOrder = changes.workOrder.currentValue as WorkOrderModel;

      this.workOrders = [workOrder];
      this.workOrders.forEach((w) => (w.isEditRow = false));

      this.workOrderForm.patchValue(workOrder);
    }
  }

  ngOnInit(): void {
    this.stepItems = [{ label: 'Work Order Details' }, { label: 'Complete' }];
  }

  onSubmit() {
    this.submitEvent.emit();
  }

  hideDialog() {
    this.workOrders = [];
    this.stepIndex = 0;
    this.workOrderForm.reset();
    this.hideDialogEvent.emit();
  }

  allowMoveToCompleteStep(workOrders: WorkOrder[]): boolean {
    const haveFilledDataRows = workOrders.filter((i) => i.quantity === 0).length === 0;
    const haveNotEditRows = workOrders.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.workOrders.length > 0;
  }

  onRowEditInit(workOrder: WorkOrder) {
    workOrder.isEditRow = true;
    this.clonedWorkOrderModels[workOrder.productId] = { ...workOrder };
  }

  onRowEditSave(workOrder: WorkOrder) {
    workOrder.isEditRow = false;
    const entity = this.workOrders.find((i) => i.productId === workOrder.productId);
    entity.quantity = workOrder.quantity;
    delete this.clonedWorkOrderModels[workOrder.productId];
  }

  onRowEditCancel(workOrder: WorkOrder, index: number) {
    this.workOrders[index] = this.clonedWorkOrderModels[workOrder.productId];
    this.workOrders[index].isEditRow = false;
    delete this.clonedWorkOrderModels[workOrder.productId];
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        const workOrder = this.workOrders && this.workOrders.length && this.workOrders[0];

        if (workOrder) {
          this.productIdControl.setValue(workOrder.productId);
          this.quantityControl.setValue(workOrder.quantity);
        }
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
