import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ProductModel } from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-shipping-plan-create',
  templateUrl: './shipping-plan-create.component.html',
  styleUrls: ['./shipping-plan-create.component.scss'],
})
export class ShippingPlanCreateComponent implements OnInit {
  @Input() shippingPlanForm: FormGroup;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;
  @Input() products: ProductModel[] = [];

  @Output() submitEvent = new EventEmitter<FormGroup>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  stepItems: MenuItem[];
  stepIndex = 0;
  minDate = new Date();

  productCols: any[] = [];
  selectedProduct: ProductModel;

  productDetails: ProductDetail[] = [];
  clonedProductDetails: { [s: string]: ProductDetail } = {};

  TypeColumn = TypeColumn;

  get customerNameControl() {
    return this.shippingPlanForm.get('customerName');
  }

  get salesOrderControl() {
    return this.shippingPlanForm.get('salesOrder');
  }

  get salelineNumberControl() {
    return this.shippingPlanForm.get('salelineNumber');
  }

  get purchaseOrderControl() {
    return this.shippingPlanForm.get('purchaseOrder');
  }

  get shippingDateControl() {
    return this.shippingPlanForm.get('shippingDate');
  }

  get billToControl() {
    return this.shippingPlanForm.get('billTo');
  }

  get billToAddressControl() {
    return this.shippingPlanForm.get('billToAddress');
  }

  get shipToControl() {
    return this.shippingPlanForm.get('shipTo');
  }

  get shipToAddressControl() {
    return this.shippingPlanForm.get('shipToAddress');
  }

  get accountNumberControl() {
    return this.shippingPlanForm.get('accountNumber');
  }

  get productLineControl() {
    return this.shippingPlanForm.get('productLine');
  }

  get quantityControl() {
    return this.shippingPlanForm.get('quantity');
  }

  get productControl() {
    return this.shippingPlanForm.get('productId');
  }

  get amountControl() {
    return this.shippingPlanForm.get('amount');
  }

  get priceControl() {
    return this.shippingPlanForm.get('price');
  }

  get shippingModeControl() {
    return this.shippingPlanForm.get('shippingMode');
  }

  get notesControl() {
    return this.shippingPlanForm.get('notes');
  }

  constructor() {}

  ngOnInit(): void {
    this.stepItems = [{ label: 'Shipping Plan Info' }, { label: 'Products' }, { label: 'Details' }, { label: 'Complete' }];

    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Description', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];
  }

  hideDialog() {
    this.stepIndex = 0;
    this.selectedProduct = null;
    this.shippingPlanForm.reset();
    this.hideDialogEvent.emit();
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        break;
      }
      case 1: {
        this.productDetails = JSON.parse(JSON.stringify([this.selectedProduct]));
        break;
      }
    }

    this.stepIndex += 1;
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  onRowEditInit(productDetail: ProductDetail) {
    productDetail.isEditRow = true;
    this.clonedProductDetails[productDetail.id] = { ...productDetail };
  }

  onRowEditSave(productDetail: ProductDetail) {
    productDetail.isEditRow = false;
    const entity = this.productDetails.find((i) => i.id === productDetail.id);
    entity.quantity = productDetail.quantity;
    entity.price = productDetail.price;
    entity.amount = productDetail.quantity * productDetail.price;
    delete this.clonedProductDetails[productDetail.id];
  }

  onRowEditCancel(productDetail: ProductDetail, index: number) {
    this.productDetails[index] = this.clonedProductDetails[productDetail.id];
    this.productDetails[index].isEditRow = false;
    delete this.clonedProductDetails[productDetail.id];
  }

  allowMoveToCompleteStep(productDetails: ProductDetail[]): boolean {
    const haveFilledDataRows = productDetails.filter((i) => i.quantity === 0 || i.price === 0 || i.amount === 0).length === 0;
    const haveNotEditRows = productDetails.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.productDetails.length > 0;
  }

  onSubmit() {
    if (this.shippingPlanForm.invalid) {
      return;
    }

    if (this.productDetails.length !== 1) {
      return;
    }

    const { quantity, price, amount, shippingMode, id } = this.productDetails[0];

    this.shippingPlanForm.patchValue({
      quantity: quantity,
      price: price,
      amount: amount,
      shippingMode: shippingMode,
      productId: id,
    });

    this.submitEvent.emit();
  }
}

export interface ProductDetail extends ProductModel {
  isEditRow?: boolean;
  quantity: number;
  price: number;
  amount: number;
  shippingMode: string;
}
