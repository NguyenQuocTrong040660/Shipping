import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel, ShippingPlanModel, ShippingRequestClients, ShippingRequestDetailModel, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import Utilities from 'app/shared/helpers/utilities';
import { ConfirmationService, MenuItem, SelectItem } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-shipping-request-edit',
  templateUrl: './shipping-request-edit.component.html',
  styleUrls: ['./shipping-request-edit.component.scss'],
})
export class ShippingRequestEditComponent implements OnInit, OnChanges {
  @Input() shippingRequestForm: FormGroup;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;
  @Input() products: ProductModel[] = [];
  @Input() selectedShippingRequest: ShippingRequestModel;

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();
  @Output() selectedShippingPlanEvent = new EventEmitter<any>();

  stepItems: MenuItem[];
  stepIndex = 0;

  selectedProducts: ProductModel[] = [];
  shippingRequestDetails: ShippingRequestDetails[] = [];
  clonedShippingRequestDetails: { [s: string]: ShippingRequestDetails } = {};

  TypeColumn = TypeColumn;
  productCols: any[] = [];
  productFields: any[] = [];

  private destroyed$ = new Subject<void>();

  get customerNameControl() {
    return this.shippingRequestForm.get('customerName');
  }

  get semlineNumberControl() {
    return this.shippingRequestForm.get('semlineNumber');
  }

  get purchaseOrderControl() {
    return this.shippingRequestForm.get('purchaseOrder');
  }

  get shippingDateControl() {
    return this.shippingRequestForm.get('shippingDate');
  }

  get salesIdControl() {
    return this.shippingRequestForm.get('salesID');
  }

  get notesControl() {
    return this.shippingRequestForm.get('notes');
  }

  get shippingRequestDetailsControl() {
    return this.shippingRequestForm.get('shippingRequestDetails') as FormArray;
  }

  constructor(private fb: FormBuilder, private shippingRequestClients: ShippingRequestClients) {}

  ngOnInit(): void {
    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Name', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.productFields = this.productCols.map((i) => i.field);

    this.stepItems = [{ label: 'Shipping Request Info' }, { label: 'Products' }, { label: 'Details' }, { label: 'Complete' }];
  }

  ngOnChanges() {
    if (!this.shippingRequestForm) return;

    this.shippingDateControl.patchValue(new Date(this.shippingDateControl.value));
    const products = this.shippingRequestDetailsControl.value.map((i) => i.product);
    this.selectedProducts = products;
  }

  handleOnSelectShippingPlan(shippingPlanId: number) {
    this.selectedShippingPlanEvent.emit(shippingPlanId);
  }

  hideDialog() {
    this.selectedProducts = [];
    this.shippingRequestDetails = [];
    this.stepIndex = 0;
    this.hideDialogEvent.emit();
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this._getDetailShippingRequest();
        break;
      }
      case 1: {
        this.shippingRequestDetails = this._mapToProductsToShippingRequestDetailModels(this.selectedProducts);

        if (this.selectedShippingRequest.shippingRequestDetails && this.selectedShippingRequest.shippingRequestDetails.length > 0) {
          this.shippingRequestDetails.forEach((item) => {
            const shippingPlanDetail = this.selectedShippingRequest.shippingRequestDetails.find((i) => item.productId === i.productId);

            if (shippingPlanDetail) {
              item.amount = shippingPlanDetail.amount;
              item.quantity = shippingPlanDetail.quantity;
              item.price = shippingPlanDetail.price;
              item.shippingMode = shippingPlanDetail.shippingMode;
            }
          });
        }

        break;
      }
    }
    this.stepIndex += 1;
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  onRowEditInit(shippingRequestDetailModel: ShippingRequestDetails) {
    shippingRequestDetailModel.isEditRow = true;
    this.clonedShippingRequestDetails[shippingRequestDetailModel.productId] = { ...shippingRequestDetailModel };
  }

  onRowDelete(shippingRequestDetailModel: ShippingRequestDetails) {
    this.selectedProducts = this.selectedProducts.filter((i) => i.id !== shippingRequestDetailModel.productId);
    this.shippingRequestDetails = this.shippingRequestDetails.filter((i) => i.productId !== shippingRequestDetailModel.productId);
  }

  onRowEditSave(shippingRequestDetailModel: ShippingRequestDetails) {
    shippingRequestDetailModel.isEditRow = false;
    const entity = this.shippingRequestDetails.find((i) => i.productId === shippingRequestDetailModel.productId);
    entity.quantity = shippingRequestDetailModel.quantity;
    entity.amount = shippingRequestDetailModel.quantity * shippingRequestDetailModel.price;
    delete this.clonedShippingRequestDetails[shippingRequestDetailModel.productId];
  }

  onRowEditCancel(shippingRequestDetailModel: ShippingRequestDetails, index: number) {
    this.shippingRequestDetails[index] = this.clonedShippingRequestDetails[shippingRequestDetailModel.productId];
    delete this.clonedShippingRequestDetails[shippingRequestDetailModel.productId];
  }

  allowMoveToCompleteStep(shippingRequestDetailModels: ShippingRequestDetails[]): boolean {
    const haveFilledDataRows = shippingRequestDetailModels.filter((i) => i.quantity === 0 || i.price === 0 || i.amount === 0).length === 0;
    const haveNotEditRows = shippingRequestDetailModels.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.shippingRequestDetails.length > 0;
  }

  onSubmit() {
    this.shippingRequestDetailsControl.clear();

    this.shippingRequestDetails.forEach((i) => {
      this.shippingRequestDetailsControl.push(this.initShippingRequestDetailForm(i));
    });

    this.submitEvent.emit();
  }

  initShippingRequestDetailForm(shippingRequestDetailModel: ShippingRequestDetails) {
    return this.fb.group({
      quantity: [shippingRequestDetailModel.quantity],
      productId: [shippingRequestDetailModel.productId],
      amount: [shippingRequestDetailModel.quantity * shippingRequestDetailModel.price],
      price: [shippingRequestDetailModel.price],
      shippingRequestId: [shippingRequestDetailModel.shippingRequestId],
      shippingMode: [shippingRequestDetailModel.shippingMode],
    });
  }

  _mapToProductsToShippingRequestDetailModels(products: ProductModel[]): ShippingRequestDetails[] {
    return products.map((item, index) => {
      return {
        id: index + 1,
        productId: item.id,
        product: item,
        shippingRequest: null,
        shippingRequestId: 0,
        quantity: 0,
        price: 0,
        amount: 0,
        shippingMode: '',
        isEditRow: false,
      };
    });
  }

  _mapToSelectShippingPlanItem(shippingPlans: ShippingPlanModel[]): SelectItem[] {
    return shippingPlans.map((p) => ({
      value: p.id,
      label: `${p.identifier} | ${p.purchaseOrder} | ${p.customerName} | ${p.salesID} | ${p.semlineNumber} | ${p.shippingDate}`,
    }));
  }

  _getDetailShippingRequest() {
    this.selectedShippingRequest.shippingRequestDetails = [];

    this.shippingRequestClients
      .getShippingRequestById(this.selectedShippingRequest.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ShippingRequestModel) => {
          this.selectedShippingRequest.shippingRequestDetails = i.shippingRequestDetails;
          const products = this.selectedShippingRequest.shippingRequestDetails.map((i) => i.product);
          this.selectedProducts = products;
        },
        (_) => (this.selectedShippingRequest.shippingRequestDetails = [])
      );
  }
}

export interface ShippingRequestDetails extends ShippingRequestDetailModel {
  isEditRow?: boolean;
}