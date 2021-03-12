import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ShippingRequestClients, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { NotificationService } from 'app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-shipping-request-documents',
  templateUrl: './shipping-request-documents.component.html',
  styleUrls: ['./shipping-request-documents.component.scss'],
})
export class ShippingRequestDocumentsComponent implements OnInit, OnChanges, OnDestroy {
  @Input() selectedShippingRequest: ShippingRequestModel;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  shippingRequestDocumentsForm: FormGroup;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;
  private destroyed$ = new Subject<void>();

  get shippingRequestIdControl() {
    return this.shippingRequestDocumentsForm.get('shippingRequestId');
  }

  get shippingRequestIdentifierControl() {
    return this.shippingRequestDocumentsForm.get('shippingRequestIdentifier');
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

  constructor(private fb: FormBuilder, private shippingRequestClients: ShippingRequestClients, private notificationService: NotificationService) {}

  ngOnInit() {
    this.initForm();
  }

  ngOnChanges() {
    if (this.selectedShippingRequest) {
      this.shippingRequestIdControl.setValue(this.selectedShippingRequest.id);
      this.shippingRequestIdentifierControl.setValue(this.selectedShippingRequest.identifier);

      this._getShippingRequestDocumentById(this.selectedShippingRequest.id);
    }
  }

  initForm() {
    this.shippingRequestDocumentsForm = this.fb.group({
      shippingRequestId: [0, [Validators.required]],
      shippingRequestIdentifier: ['', [Validators.required]],
      customDeclarationNumber: [''],
      grossWeight: [0],
      billToCustomer: [''],
      receiverCustomer: [''],
      trackingNumber: [''],
      receiverAddress: [''],
      notes: [''],
    });
  }

  hideDialog() {
    this.hideDialogEvent.emit();
  }

  onSubmit() {
    this._updateShippingRequestDocument();
  }

  _getShippingRequestDocumentById(shippingRequestId: number) {
    this.shippingRequestClients
      .getShippingRequestLogistic(shippingRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.shippingRequestDocumentsForm.patchValue(i);
        },
        (_) => this.notificationService.error('Failed to Get Shipping Request Document')
      );
  }

  _updateShippingRequestDocument() {
    this.shippingRequestClients
      .updateShippingRequestLogistic(this.shippingRequestIdControl.value, this.shippingRequestDocumentsForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Update Shipping Request Document Successfully');
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => this.notificationService.error('Failed to Update Shipping Request Document')
      );
  }

  ngOnDestroy() {
    this.destroyed$.next();
    this.destroyed$.unsubscribe();
  }
}
