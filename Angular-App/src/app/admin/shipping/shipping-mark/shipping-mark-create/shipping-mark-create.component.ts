import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel, ReceivedMarkPrintingModel, ShippingMarkShippingModel, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import Utilities from 'app/shared/helpers/utilities';
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

  dataSummary: any[] = [];

  get notesControl() {
    return this.shippingMarkForm.get('notes');
  }

  get shippingRequestControl() {
    return this.shippingMarkForm.get('shippingRequest');
  }

  get shippingMarkShippingsControl() {
    return this.shippingMarkForm.get('shippingMarkShippings') as FormArray;
  }

  get receivedMarkPrintingsControl() {
    return this.shippingMarkForm.get('receivedMarkPrintings') as FormArray;
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
      label: `Customer: ${p.customerName} | Bill To: ${p.billTo} | Bill To: ${p.shipTo}| Shipping Date: ${Utilities.ConvertDateBeforeSendToServer(p.shippingDate)
        .toISOString()
        .split('T')[0]
        .split('-')
        .reverse()
        .join('/')}`,
    }));
  }

  canNavigateToSummaryStep() {
    return (
      this.shippingMarkShippings.filter(
        (item) =>
          (item['selectedReceivedMarks'] && item['selectedReceivedMarks'].length === 0) || item.quantity !== this.calculateQuantityReceivedMark(item['selectedReceivedMarks'])
      ).length > 0
    );
  }

  onSubmit() {
    this.shippingMarkShippingsControl.clear();
    this.receivedMarkPrintingsControl.clear();

    this.shippingMarkShippings.forEach((i) => {
      this.shippingMarkShippingsControl.push(this.initShippingMarkShippingForm(i));
      i['selectedReceivedMarks'].forEach((receivedMark: ReceivedMarkPrintingModel) => {
        this.receivedMarkPrintingsControl.push(this.initReceivedMarkPrintingForm(receivedMark));
      });
    });

    this.submitEvent.emit();
  }

  initShippingMarkShippingForm(shippingMarkShipping: ShippingMarkShippingModel) {
    return this.fb.group({
      quantity: [shippingMarkShipping.quantity],
      productId: [shippingMarkShipping.productId],
      shippingRequestId: [shippingMarkShipping.shippingRequestId],
      shippingMarkId: [0],
    });
  }

  initReceivedMarkPrintingForm(receivedMarkPrinting: ReceivedMarkPrintingModel) {
    return this.fb.group({
      id: [receivedMarkPrinting.id],
      productId: [receivedMarkPrinting.productId],
      receivedMarkId: [receivedMarkPrinting.receivedMarkId],
      shippingMarkId: [0],
      quantity: [receivedMarkPrinting.quantity],
    });
  }

  handleSelectedShippingRequest(shippingRequest: ShippingRequestModel) {
    this.selectedShippingRequest = shippingRequest;
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.selectedShippingRequestEvent.emit(this.selectedShippingRequest);
        this.stepIndex += 1;
        break;
      }
      case 1: {
        this.dataSummary = [];
        this.stepIndex += 1;

        this.shippingMarkShippings.forEach((item: ShippingMarkShippingModel) => {
          const totalReceivedMarks = this.calculateQuantityReceivedMark(item['selectedReceivedMarks']);

          const record = {
            productName: item.product.productName,
            productNumber: item.product.productNumber,
            quantity: item.quantity,
            qtyPerPackage: item.product.qtyPerPackage,
            totalReceivedMarks: totalReceivedMarks,
            totalShippingMarks: this.calculateShippingMarks(item.product, totalReceivedMarks),
          };

          this.dataSummary.push(record);
        });

        break;
      }
    }
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  calculateQuantityReceivedMark(selectedReceivedMarks: ReceivedMarkPrintingModel[]) {
    return selectedReceivedMarks.reduce((i, j) => i + j.quantity, 0);
  }

  calculateShippingMarks(product: ProductModel, totalQuantityOfReceivedMarks: number) {
    let remainTotal = totalQuantityOfReceivedMarks;
    let totalShippingMarks = 0;
    const qtyPerPackageNumber = parseInt(product.qtyPerPackage, 0) * 1;

    while (remainTotal > 0) {
      if (remainTotal >= qtyPerPackageNumber) {
        totalShippingMarks++;
      } else {
        totalShippingMarks++;
      }

      remainTotal = remainTotal - qtyPerPackageNumber;
    }

    return totalShippingMarks;
  }
}
