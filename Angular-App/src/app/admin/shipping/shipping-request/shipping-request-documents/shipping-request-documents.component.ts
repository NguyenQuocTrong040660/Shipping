import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ShippingRequestClients, ShippingRequestDetailModel, ShippingRequestLogisticModel } from 'app/shared/api-clients/shipping-app.client';
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
  @Input() selectedShippingRequestDetail: ShippingRequestDetailModel;

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

  get productIdControl() {
    return this.shippingRequestDocumentsForm.get('productId');
  }

  get customDeclarationNumberControl() {
    return this.shippingRequestDocumentsForm.get('customDeclarationNumber');
  }

  get grossWeightControl() {
    return this.shippingRequestDocumentsForm.get('grossWeight');
  }

  get trackingNumberControl() {
    return this.shippingRequestDocumentsForm.get('trackingNumber');
  }

  get notesControl() {
    return this.shippingRequestDocumentsForm.get('notes');
  }

  get forwarderControl() {
    return this.shippingRequestDocumentsForm.get('forwarder');
  }

  get dimensionControl() {
    return this.shippingRequestDocumentsForm.get('dimension');
  }

  get netWeightControl() {
    return this.shippingRequestDocumentsForm.get('netWeight');
  }

  get totalPackagesControl() {
    return this.shippingRequestDocumentsForm.get('totalPackages');
  }

  constructor(private fb: FormBuilder, private shippingRequestClients: ShippingRequestClients, private notificationService: NotificationService) {}

  ngOnInit() {
    this.initForm();
  }

  ngOnChanges() {
    if (this.selectedShippingRequestDetail) {
      this._getShippingRequestDocument(this.selectedShippingRequestDetail.shippingRequestId, this.selectedShippingRequestDetail.productId);
    }
  }

  initForm() {
    this.shippingRequestDocumentsForm = this.fb.group({
      id: [0],
      shippingRequestId: [0, [Validators.required]],
      customDeclarationNumber: ['', [Validators.required]],
      trackingNumber: ['', [Validators.required]],
      netWeight: [0, [Validators.required]],
      grossWeight: [0, [Validators.required]],
      forwarder: ['', [Validators.required]],
      dimension: ['', [Validators.required]],
      totalPackages: ['', [Validators.required]],
      notes: [''],
      productId: [0, [Validators.required]],
      lastModified: [null],
      lastModifiedBy: [null],
      product: [null],
      shippingRequest: [null],
    });
  }

  hideDialog() {
    this.hideDialogEvent.emit();
  }

  onSubmit() {
    this._updateShippingRequestDocument(
      this.selectedShippingRequestDetail.shippingRequestId,
      this.selectedShippingRequestDetail.productId,
      this.shippingRequestDocumentsForm.value
    );
  }

  _getShippingRequestDocument(shippingRequestId: number, productId: number) {
    this.shippingRequestClients
      .getShippingRequestLogistic(shippingRequestId, productId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.shippingRequestDocumentsForm.setValue(i);
        },
        (_) => this.notificationService.error('Failed to Get Shipping Request Document')
      );
  }

  _updateShippingRequestDocument(shippingRequestId: number, productId: number, shippingDocument: ShippingRequestLogisticModel) {
    this.shippingRequestClients
      .updateShippingRequestLogistic(shippingRequestId, productId, shippingDocument)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Update Shipping Request Document Successfully');
            this.shippingRequestDocumentsForm.reset();
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
