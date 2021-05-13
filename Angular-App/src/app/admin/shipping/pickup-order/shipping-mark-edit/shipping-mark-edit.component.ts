import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel, ReceivedMarkPrintingModel, ShippingMarkModel, ShippingMarkShippingModel, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-shipping-mark-edit',
  templateUrl: './shipping-mark-edit.component.html',
  styleUrls: ['./shipping-mark-edit.component.scss'],
})
export class ShippingMarkEditComponent implements OnInit {
  @Input() shippingMarkForm: FormGroup;
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() shippingMark: ShippingMarkModel;

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  stepItems: MenuItem[];
  stepIndex = 0;

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

  ngOnInit(): void {
    this.stepItems = [{ label: 'Products' }, { label: 'Summary' }];
  }

  hideDialog() {
    this.shippingMark = null;
    this.stepIndex = 0;
    this.shippingMarkForm.reset();
    this.hideDialogEvent.emit();
  }

  canNavigateToSummaryStep() {
    return this.shippingMark.shippingMarkShippings.filter((item) => item['selectedReceivedMarks'] && item['selectedReceivedMarks'].length === 0).length > 0;
  }

  onSubmit() {
    this.shippingMarkShippingsControl.clear();
    this.receivedMarkPrintingsControl.clear();

    this.shippingMark.shippingMarkShippings.forEach((i) => {
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
      shippingMarkId: [shippingMarkShipping.shippingMarkId],
    });
  }

  initReceivedMarkPrintingForm(receivedMarkPrinting: ReceivedMarkPrintingModel) {
    return this.fb.group({
      id: [receivedMarkPrinting.id],
      productId: [receivedMarkPrinting.productId],
      receivedMarkId: [receivedMarkPrinting.receivedMarkId],
      shippingMarkId: [receivedMarkPrinting.shippingMarkId],
      quantity: [receivedMarkPrinting.quantity],
    });
  }

  handleSelectedShippingRequest(shippingRequest: ShippingRequestModel) {
    this.selectedShippingRequest = shippingRequest;
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.dataSummary = [];
        this.stepIndex += 1;

        this.shippingMark.shippingMarkShippings.forEach((item: ShippingMarkShippingModel) => {
          const totalReceivedMarks = this.calculateQuantityReceivedMark(item['selectedReceivedMarks']);

          const record = {
            productName: item.product.productName,
            productNumber: item.product.productNumber,
            qtyPerPackage: item.product.qtyPerPackage,
            quantity: item.quantity,
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
