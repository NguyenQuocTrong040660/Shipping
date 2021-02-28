import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MovementRequestDetailModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-movement-request-create',
  templateUrl: './movement-request-create.component.html',
  styleUrls: ['./movement-request-create.component.scss'],
})
export class MovementRequestCreateComponent implements OnInit {
  @Input() movementRequestForm: FormGroup;
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() movementRequestDetails: MovementRequestDetailModel[] = [];
  @Input() selectItems: SelectItem[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() selectedWorkOrdersEvent = new EventEmitter<any>();

  clonedMovementRequestDetailModels: { [s: string]: MovementRequestDetailModel } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  get notesControl() {
    return this.movementRequestForm.get('notes');
  }

  get workOrdersControl() {
    return this.movementRequestForm.get('workOrders');
  }

  get movementRequestDetailsControl() {
    return this.movementRequestForm.get('movementRequestDetails') as FormArray;
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.stepItems = [{ label: 'Work Orders' }, { label: 'Confirmation' }, { label: 'Complete' }];
  }

  hideDialog() {
    this.movementRequestDetails = [];
    this.stepIndex = 0;
    this.movementRequestForm.reset();
    this.hideDialogEvent.emit();
  }

  onSubmit() {
    this.movementRequestDetailsControl.clear();

    this.movementRequestDetails.forEach((i) => {
      this.movementRequestDetailsControl.push(this.initMovementRequestDetailForm(i));
    });

    this.submitEvent.emit();
  }

  initMovementRequestDetailForm(movementRequestDetail: MovementRequestDetailModel) {
    return this.fb.group({
      quantity: [movementRequestDetail.quantity],
      productId: [movementRequestDetail.productId],
      workOrderId: [movementRequestDetail.workOrderId],
      movementRequestId: [movementRequestDetail.movementRequestId],
    });
  }

  checkModifiedQuantity(movementRequestDetails: MovementRequestDetailModel[]) {
    return movementRequestDetails.filter((i) => i.quantity === 0).length === 0;
  }

  onRowEditInit(movementRequestDetailModel: MovementRequestDetailModel) {
    const key = `${movementRequestDetailModel.workOrderId}-${movementRequestDetailModel.productId}`;
    this.clonedMovementRequestDetailModels[key] = { ...movementRequestDetailModel };
  }

  onRowDelete(movementRequestDetailModel: MovementRequestDetailModel) {
    this.movementRequestDetails = this.movementRequestDetails.filter((i) => i['id'] !== movementRequestDetailModel['id']);
  }

  onRowEditSave(movementRequestDetailModel: MovementRequestDetailModel) {
    const entity = this.movementRequestDetails.find((i) => i['id'] === movementRequestDetailModel['id']);
    entity.quantity = movementRequestDetailModel.quantity;
    delete this.clonedMovementRequestDetailModels[movementRequestDetailModel['id']];
  }

  onRowEditCancel(movementRequestDetailModel: MovementRequestDetailModel, index: number) {
    const key = `${movementRequestDetailModel.workOrderId}-${movementRequestDetailModel.productId}`;

    this.movementRequestDetails[index] = this.clonedMovementRequestDetailModels[key];
    delete this.clonedMovementRequestDetailModels[key];
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.selectedWorkOrdersEvent.emit();
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
