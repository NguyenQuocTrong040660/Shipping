import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MovementRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { MenuItem, SelectItem } from 'primeng/api';
import { ReceivedMarkMovement } from '../received-mark.component';

@Component({
  selector: 'app-received-mark-create',
  templateUrl: './received-mark-create.component.html',
  styleUrls: ['./received-mark-create.component.scss'],
})
export class ReceivedMarkCreateComponent implements OnInit, OnChanges {
  @Input() receivedMarkForm: FormGroup;
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() receivedMarkMovements: ReceivedMarkMovement[] = [];
  @Input() movementRequests: MovementRequestModel[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() selectedMovementRequestsEvent = new EventEmitter<any>();

  clonedReceivedMarkMovementModels: { [s: string]: ReceivedMarkMovement } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  selecteMovementRequestItems: SelectItem[] = [];
  dataGroupByProduct = [];

  get notesControl() {
    return this.receivedMarkForm.get('notes');
  }

  get movementRequestsControl() {
    return this.receivedMarkForm.get('movementRequests');
  }

  get receivedMarkMovementsControl() {
    return this.receivedMarkForm.get('receivedMarkMovements') as FormArray;
  }

  constructor(private fb: FormBuilder) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.movementRequests && changes.movementRequests.currentValue) {
      this.selecteMovementRequestItems = this._mapDataToMovementRequestItems(changes.movementRequests.currentValue);
    }

    if (changes.receivedMarkMovements && changes.receivedMarkMovements.currentValue.length > 0) {
      this.receivedMarkMovements.forEach((r) => (r.isEditRow = false));
    }
  }

  ngOnInit(): void {
    this.stepItems = [{ label: 'Movement Requests' }, { label: 'Products' }, { label: 'Summary' }];
  }

  hideDialog() {
    this.receivedMarkMovements = [];
    this.stepIndex = 0;
    this.receivedMarkForm.reset();
    this.hideDialogEvent.emit();
  }

  _mapDataToMovementRequestItems(movementRequests: MovementRequestModel[]): SelectItem[] {
    return movementRequests.map((p) => ({
      value: p,
      label: `Movement Requests: ${p.workOrdersCollection} SEQ -${p.id} `,
    }));
  }

  onSubmit() {
    this.receivedMarkMovementsControl.clear();

    this.receivedMarkMovements.forEach((i) => {
      this.receivedMarkMovementsControl.push(this.initReceivedMarkMovementForm(i));
    });

    this.submitEvent.emit();
  }

  initReceivedMarkMovementForm(receivedMarkMovement: ReceivedMarkMovement) {
    return this.fb.group({
      quantity: [receivedMarkMovement.quantity],
      productId: [receivedMarkMovement.productId],
      movementRequestId: [receivedMarkMovement.movementRequestId],
      receivedMarkId: 0,
    });
  }

  allowMoveToCompleteStep(receivedMarkMovements: ReceivedMarkMovement[]): boolean {
    const haveFilledDataRows = receivedMarkMovements.filter((i) => i.quantity === 0).length === 0;
    const haveNotEditRows = receivedMarkMovements.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.receivedMarkMovements.length > 0;
  }

  onRowEditInit(receivedMarkMovement: ReceivedMarkMovement) {
    receivedMarkMovement.isEditRow = true;
    this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']] = { ...receivedMarkMovement };
  }

  onRowDelete(receivedMarkMovement: ReceivedMarkMovement) {
    this.receivedMarkMovements = this.receivedMarkMovements.filter((i) => i['id'] !== receivedMarkMovement['id']);
  }

  onRowEditSave(receivedMarkMovement: ReceivedMarkMovement) {
    receivedMarkMovement.isEditRow = false;
    const entity = this.receivedMarkMovements.find((i) => i['id'] === receivedMarkMovement['id']);
    entity.quantity = receivedMarkMovement.quantity;
    delete this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
  }

  onRowEditCancel(receivedMarkMovement: ReceivedMarkMovement, index: number) {
    this.receivedMarkMovements[index] = this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
    this.receivedMarkMovements[index].isEditRow = false;
    delete this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.selectedMovementRequestsEvent.emit();
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
