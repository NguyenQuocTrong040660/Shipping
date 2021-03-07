import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ReceivedMarkModel, ReceivedMarkMovementModel } from 'app/shared/api-clients/shipping-app.client';
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
  @Input() receivedMark: ReceivedMarkModel;

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
    this.receivedMark = null;
    this.stepIndex = 0;
    this.receivedMarkForm.reset();
    this.hideDialogEvent.emit();
  }

  onSubmit() {
    this.receivedMarkMovementsControl.clear();

    this.receivedMark.receivedMarkMovements.forEach((i) => {
      this.receivedMarkMovementsControl.push(this.initReceivedMarkMovementForm(i));
    });

    this.submitEvent.emit();
  }

  initReceivedMarkMovementForm(receivedMarkMovement: ReceivedMarkMovementModel) {
    return this.fb.group({
      quantity: [receivedMarkMovement.quantity],
      productId: [receivedMarkMovement.productId],
      movementRequestId: [receivedMarkMovement.movementRequestId],
      receivedMarkId: [receivedMarkMovement.receivedMarkId],
    });
  }

  checkModifiedQuantity(receivedMarkMovements: ReceivedMarkMovementModel[]) {
    return receivedMarkMovements.filter((i) => i.quantity === 0).length === 0;
  }

  onRowEditInit(receivedMarkMovement: ReceivedMarkMovementModel) {
    this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']] = { ...receivedMarkMovement };
  }

  onRowDelete(receivedMarkMovement: ReceivedMarkMovementModel) {
    this.receivedMark.receivedMarkMovements = this.receivedMark.receivedMarkMovements.filter((i) => i['id'] !== receivedMarkMovement['id']);
  }

  onRowEditSave(receivedMarkMovement: ReceivedMarkMovementModel) {
    const entity = this.receivedMark.receivedMarkMovements.find((i) => i['id'] === receivedMarkMovement['id']);
    entity.quantity = receivedMarkMovement.quantity;
    delete this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
  }

  onRowEditCancel(receivedMarkMovement: ReceivedMarkMovementModel, index: number) {
    this.receivedMark.receivedMarkMovements[index] = this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
    delete this.clonedReceivedMarkMovementModels[receivedMarkMovement['id']];
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.dataGroupByProduct = [];

        const products = this.receivedMark.receivedMarkMovements.map((i) => i.product);
        const productNumbers = this.receivedMark.receivedMarkMovements.map((i) => i.product.productNumber);
        let uniqueProducts = [...new Set(productNumbers)];
        uniqueProducts.forEach((item) => {
          const product = {
            productNumber: item,
            productName: products.find((i) => i.productNumber === item).productName,
            qtyPerPackage: products.find((i) => i.productNumber === item).qtyPerPackage,
            quantity: this.receivedMark.receivedMarkMovements.map((i) => i.product.productNumber === item && i.quantity).reduce((a: number, b: number) => a + b, 0),
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
