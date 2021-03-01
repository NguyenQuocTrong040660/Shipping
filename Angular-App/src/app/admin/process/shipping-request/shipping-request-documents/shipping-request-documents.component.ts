import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';

@Component({
  selector: 'app-shipping-request-documents',
  templateUrl: './shipping-request-documents.component.html',
  styleUrls: ['./shipping-request-documents.component.scss'],
})
export class ShippingRequestDocumentsComponent implements OnInit, OnChanges {
  @Input() selectedShippingRequest: ShippingRequestModel;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  shippingRequestDocumentsForm: FormGroup;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  get idControl() {
    return this.shippingRequestDocumentsForm.get('id');
  }

  get customDeclarationNumberControl() {
    return this.shippingRequestDocumentsForm.get('customDeclarationNumber');
  }

  get grossWeightControl() {
    return this.shippingRequestDocumentsForm.get('grossWeight');
  }

  get billToCustomerControl() {
    return this.shippingRequestDocumentsForm.get('billToCustomer');
  }

  get receiverCustomerControl() {
    return this.shippingRequestDocumentsForm.get('receiverCustomer');
  }

  get receiverAddressControl() {
    return this.shippingRequestDocumentsForm.get('receiverAddress');
  }

  get trackingNumberControl() {
    return this.shippingRequestDocumentsForm.get('trackingNumber');
  }

  get notesControl() {
    return this.shippingRequestDocumentsForm.get('notes');
  }

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.initForm();
  }

  ngOnChanges() {

  }

  initForm() {
    this.shippingRequestDocumentsForm = this.fb.group({
      id: [0, [Validators.required]],
      customDeclarationNumber: [''],
      grossWeight: [0],
      billToCustomer: [''],
      receiverCustomer: [''],
      trackingNumber: [''],
      receiverAddress: [''],
      notes: ['']
    });
  }

  hideDialog() {
    this.hideDialogEvent.emit();
  }

  onSubmit() {
    this.submitEvent.emit();
  }
}