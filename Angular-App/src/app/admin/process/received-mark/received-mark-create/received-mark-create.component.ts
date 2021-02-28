import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MovementRequestModel, ReceivedMarkMovementModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-received-mark-create',
  templateUrl: './received-mark-create.component.html',
  styleUrls: ['./received-mark-create.component.scss'],
})
export class ReceivedMarkCreateComponent implements OnInit, OnChanges {
  @Input() receivedMarkForm: FormGroup;
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() receivedMarkMovements: ReceivedMarkMovementModel[] = [];
  @Input() movementRequests: MovementRequestModel[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() selectedMovementRequestsEvent = new EventEmitter<any>();

  clonedReceivedMarkMovementModels: { [s: string]: ReceivedMarkMovementModel } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  selecteMovementRequestItems: SelectItem[] = [];

  get notesControl() {
    return this.receivedMarkForm.get('notes');
  }

  get movementRequestsControl() {
    return this.receivedMarkForm.get('movementRequests');
  }

  get receivedMarkMovementsControl() {
    return this.receivedMarkForm.get('receivedMarkMovements') as FormArray;
  }

  constructor(private fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.movementRequests && changes.movementRequests.currentValue) {
      this.selecteMovementRequestItems = this._mapDataToMovementRequestItems(changes.movementRequests.currentValue);
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
      label: `${p.identifier}`,
    }));
  }

  onSubmit() {
    this.receivedMarkMovementsControl.clear();

    this.receivedMarkMovements.forEach((i) => {
      this.receivedMarkMovementsControl.push(this.initReceivedMarkMovementForm(i));
    });

    this.submitEvent.emit();
  }

  initReceivedMarkMovementForm(receivedMarkMovement: ReceivedMarkMovementModel) {
    return this.fb.group({
      quantity: [receivedMarkMovement.quantity],
      productId: [receivedMarkMovement.productId],
      movementRequestId: [receivedMarkMovement.movementRequestId],
      receivedMarkId: 0,
    });
  }

  checkModifiedQuantity(receivedMarkMovements: ReceivedMarkMovementModel[]) {
    return receivedMarkMovements.filter((i) => i.quantity === 0).length === 0;
  }

  onRowEditInit(receivedMarkMovement: ReceivedMarkMovementModel) {
    this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']] = { ...receivedMarkMovement };
  }

  onRowDelete(receivedMarkMovement: ReceivedMarkMovementModel) {
    this.receivedMarkMovements = this.receivedMarkMovements.filter((i) => i['id'] !== receivedMarkMovement['id']);
  }

  onRowEditSave(receivedMarkMovement: ReceivedMarkMovementModel) {
    const entity = this.receivedMarkMovements.find((i) => i['id'] === receivedMarkMovement['id']);
    entity.quantity = receivedMarkMovement.quantity;
    delete this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
  }

  onRowEditCancel(receivedMarkMovement: ReceivedMarkMovementModel, index: number) {
    this.receivedMarkMovements[index] = this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
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
