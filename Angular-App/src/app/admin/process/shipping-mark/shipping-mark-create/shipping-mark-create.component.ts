import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ShippingMarkShippingModel, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { MenuItem, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-shipping-mark-create',
  templateUrl: './shipping-mark-create.component.html',
  styleUrls: ['./shipping-mark-create.component.scss'],
})
export class ShippingMarkCreateComponent implements OnInit, OnChanges {
  @Input() shippingMarkForm: FormGroup;
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() shippingMarkShippings: ShippingMarkShippingModel[] = [];
  @Input() shippingRequests: ShippingRequestModel[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() selectedShippingRequestEvent = new EventEmitter<any>();

  stepItems: MenuItem[];
  stepIndex = 0;

  selecteShippingRequestItems: SelectItem[] = [];
  selectedShippingRequest: ShippingRequestModel;

  selectedItems: any[];

  get notesControl() {
    return this.shippingMarkForm.get('notes');
  }

  get shippingRequestControl() {
    return this.shippingMarkForm.get('shippingRequest');
  }

  get shippingMarkShippingsControl() {
    return this.shippingMarkForm.get('shippingMarkShippings') as FormArray;
  }

  constructor(private fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.shippingRequests && changes.shippingRequests.currentValue) {
      this.selecteShippingRequestItems = this._mapDataToShippingRequestItems(changes.shippingRequests.currentValue);
    }
  }

  ngOnInit(): void {
    this.stepItems = [{ label: 'Shipping Requests' }, { label: 'Products' }, { label: 'Summary' }];
  }

  hideDialog() {
    this.shippingMarkShippings = [];
    this.stepIndex = 0;
    this.shippingMarkForm.reset();
    this.hideDialogEvent.emit();
  }

  _mapDataToShippingRequestItems(shippingRequests: ShippingRequestModel[]): SelectItem[] {
    return shippingRequests.map((p) => ({
      value: p,
      label: `${p.identifier}`,
    }));
  }

  onSubmit() {
    this.shippingMarkShippingsControl.clear();

    this.shippingMarkShippings.forEach((i) => {
      this.shippingMarkShippingsControl.push(this.initShippingMarkMovementForm(i));
    });

    this.submitEvent.emit();
  }

  initShippingMarkMovementForm(shippingMarkShipping: ShippingMarkShippingModel) {
    return this.fb.group({
      quantity: [shippingMarkShipping.quantity],
      productId: [shippingMarkShipping.productId],
      shippingRequestId: [shippingMarkShipping.shippingRequestId],
      shippingMarkId: 0,
    });
  }

  handleSelectedShippingRequest(shippingRequest: ShippingRequestModel) {
    this.selectedShippingRequest = shippingRequest;
  }

  handleSelectReceivedMarkPrinting($event) {
    console.log($event);
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.selectedShippingRequestEvent.emit(this.selectedShippingRequest);
        this.stepIndex += 1;
        break;
      }
      case 1: {
        this.stepIndex += 1;
        break;
      }
    }
  }

  prevPage() {
    this.stepIndex -= 1;
  }
}
