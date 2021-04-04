import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem, SelectItem } from 'primeng/api';
import { MovementRequestDetail } from '../movement-request.component';

@Component({
  selector: 'app-movement-request-create',
  templateUrl: './movement-request-create.component.html',
  styleUrls: ['./movement-request-create.component.scss'],
})
export class MovementRequestCreateComponent implements OnInit, OnChanges {
  @Input() movementRequestForm: FormGroup;
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() movementRequestDetails: MovementRequestDetail[] = [];
  @Input() selectItems: SelectItem[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() selectedWorkOrdersEvent = new EventEmitter<any>();

  clonedMovementRequestDetailModels: { [s: string]: MovementRequestDetail } = {};

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

  ngOnChanges(changes: SimpleChanges) {
    if (changes.movementRequestDetails && this.movementRequestDetails && this.movementRequestDetails.length > 0) {
      this.movementRequestDetails.forEach((m) => (m.isEditRow = false));
    }
  }

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

  initMovementRequestDetailForm(movementRequestDetail: MovementRequestDetail) {
    return this.fb.group({
      quantity: [movementRequestDetail.quantity],
      productId: [movementRequestDetail.productId],
      workOrderId: [movementRequestDetail.workOrderId],
      isDirect: [movementRequestDetail.isDirect],
      movementRequestId: [movementRequestDetail.movementRequestId],
    });
  }

  allowMoveToCompleteStep(movementRequestDetails: MovementRequestDetail[]): boolean {
    const haveFilledDataRows = movementRequestDetails.filter((i) => i.quantity === 0).length === 0;
    const haveNotEditRows = movementRequestDetails.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.movementRequestDetails.length > 0;
  }

  onRowEditInit(movementRequestDetailModel: MovementRequestDetail) {
    movementRequestDetailModel.isEditRow = true;
    const key = `${movementRequestDetailModel.workOrderId}-${movementRequestDetailModel.productId}`;
    this.clonedMovementRequestDetailModels[key] = { ...movementRequestDetailModel };
  }

  onRowDelete(movementRequestDetailModel: MovementRequestDetail) {
    this.movementRequestDetails = this.movementRequestDetails.filter((i) => i['id'] !== movementRequestDetailModel['id']);
  }

  onRowEditSave(movementRequestDetailModel: MovementRequestDetail) {
    movementRequestDetailModel.isEditRow = false;
    const entity = this.movementRequestDetails.find((i) => i['id'] === movementRequestDetailModel['id']);
    entity.quantity = movementRequestDetailModel.quantity;
    delete this.clonedMovementRequestDetailModels[movementRequestDetailModel['id']];
  }

  onRowEditCancel(movementRequestDetailModel: MovementRequestDetail, index: number) {
    const key = `${movementRequestDetailModel.workOrderId}-${movementRequestDetailModel.productId}`;

    this.movementRequestDetails[index] = this.clonedMovementRequestDetailModels[key];
    this.movementRequestDetails[index].isEditRow = false;
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
