import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';

@Component({
  selector: 'app-shipping-request-documents',
  templateUrl: './shipping-request-documents.component.html',
  styleUrls: ['./shipping-request-documents.component.scss'],
})
export class ShippingRequestDocumentsComponent implements OnInit {
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() shippingRequest = null;

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  isViewDocuments = true;

  shippingRequestLogisticForm: FormGroup;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  get notesControl() {
    return this.shippingRequestLogisticForm.get('notes');
  }

  get receiverAddressControl() {
    return this.shippingRequestLogisticForm.get('receiverAddressControl');
  }

  get trackingNumberControl() {
    return this.shippingRequestLogisticForm.get('trackingNumber');
  }

  get customDeclarationNumberControl() {
    return this.shippingRequestLogisticForm.get('customDeclarationNumber');
  }

  get receiverCustomerControl() {
    return this.shippingRequestLogisticForm.get('receiverCustomer');
  }

  get grossWeightControl() {
    return this.shippingRequestLogisticForm.get('grossWeight');
  }

  get unstuffQuantityControl() {
    return this.shippingRequestLogisticForm.get('unstuffQuantity');
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
  }

  hideDialog() {
    this.shippingRequestLogisticForm.reset();
    this.hideDialogEvent.emit();
  }

  initForm() {
    this.shippingRequestLogisticForm = this.fb.group({
      id: [0],
      grossWeight: [0, [Validators.required]],
      billToCustomer: ['', [Validators.required]],
      receiverCustomer: ['', [Validators.required]],
      customDeclarationNumber: ['', [Validators.required]],
      trackingNumber: ['', [Validators.required]],
      shippingRequestId: [0],
      receiverAddress: ['', [Validators.required]],
      notes: [''],
    });
  }

  onSubmit() {
    this.submitEvent.emit();
  }
}
