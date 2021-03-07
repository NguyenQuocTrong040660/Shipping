import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MovementRequestModel, ReceivedMarkMovementModel } from 'app/shared/api-clients/shipping-app.client';
import { MenuItem, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-received-mark-edit',
  templateUrl: './received-mark-edit.component.html',
  styleUrls: ['./received-mark-edit.component.scss'],
})
export class ReceivedMarkEditComponent implements OnInit {
  @Input() receivedMarkForm: FormGroup;
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() receivedMarkMovements: ReceivedMarkMovementModel[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  clonedReceivedMarkMovementModels: { [s: string]: ReceivedMarkMovementModel } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  selecteMovementRequestItems: SelectItem[] = [];
  dataGroupByProduct = [];

  get notesControl() {
    return this.receivedMarkForm.get('notes');
  }

  get receivedMarkMovementsControl() {
    return this.receivedMarkForm.get('receivedMarkMovements') as FormArray;
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.stepItems = [{ label: 'Products' }, { label: 'Summary' }];
  }

  hideDialog() {
    this.receivedMarkMovements = [];
    this.stepIndex = 0;
    this.receivedMarkForm.reset();
    this.hideDialogEvent.emit();
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
        this.dataGroupByProduct = [];

        const products = this.receivedMarkMovements.map((i) => i.product);
        const productNumbers = this.receivedMarkMovements.map((i) => i.product.productNumber);
        let uniqueProducts = [...new Set(productNumbers)];
        uniqueProducts.forEach((item) => {
          const product = {
            productNumber: item,
            productName: products.find((i) => i.productNumber === item).productName,
            quantity: this.receivedMarkMovements.map((i) => i.product.productNumber === item && i.quantity).reduce((a: number, b: number) => a + b, 0),
          };

          this.dataGroupByProduct.push(product);
        });

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
