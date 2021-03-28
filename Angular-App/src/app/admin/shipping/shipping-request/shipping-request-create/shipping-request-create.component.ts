import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel, ShippingPlanModel, ShippingRequestDetailModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import Utilities from 'app/shared/helpers/utilities';
import { MenuItem, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-shipping-request-create',
  templateUrl: './shipping-request-create.component.html',
  styleUrls: ['./shipping-request-create.component.scss'],
})
export class ShippingRequestCreateComponent implements OnInit {
  @Input() shippingRequestForm: FormGroup;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;
  products: ProductModel[] = [];
  selectedShippingPlans: ShippingPlanModel[] = [];
  @Input() shippingPlans: ShippingPlanModel;

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  stepItems: MenuItem[];
  stepIndex = 0;

  selectedProducts: ProductModel[] = [];
  shippingRequestDetails: ShippingRequestDetailModel[] = [];

  TypeColumn = TypeColumn;
  productCols: any[] = [];
  productFields: any[] = [];

  get shippingRequestDetailsControl() {
    return this.shippingRequestForm.get('shippingRequestDetails') as FormArray;
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Description', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.productFields = this.productCols.map((i) => i.field);

    this.stepItems = [{ label: 'Select Shipping Plan' }, { label: 'Products' }, { label: 'Shipping Request Info' }];
  }

  hideDialog() {
    this.selectedProducts = [];
    this.shippingRequestDetails = [];
    this.selectedShippingPlans = null;
    this.stepIndex = 0;
    this.hideDialogEvent.emit();
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        if (!this.selectedShippingPlans || this.selectedShippingPlans.length === 0) {
          return;
        }

        const products = this.selectedShippingPlans.map((i) => i.product);
        this.selectedProducts = products;

        this.stepIndex += 1;
        break;
      }
      case 1: {
        this.shippingRequestDetails = this._mapToProductsToShippingRequestDetailModels(this.selectedProducts);

        this.shippingRequestDetails.forEach((item) => {
          const shippingPlan = this.selectedShippingPlans.find((i) => i.product.id === item.productId);

          if (shippingPlan) {
            item.amount = shippingPlan.shippingPlanDetail.amount;
            item.quantity = shippingPlan.shippingPlanDetail.quantity;
            item.price = shippingPlan.shippingPlanDetail.price;
            item.shippingMode = shippingPlan.shippingPlanDetail.shippingMode;
            item.semlineNumber = shippingPlan.semlineNumber;
            item.purchaseOrder = shippingPlan.purchaseOrder;
            item.salesID = shippingPlan.salesID;
            item.customerName = shippingPlan.customerName;
            item.accountNumber = shippingPlan.accountNumber;
            item.productLine = shippingPlan.productLine;
            item.shippingDate = new Date(shippingPlan.shippingDate);
          }
        });

        this.stepIndex += 1;
        break;
      }

      case 2:
        this.stepIndex += 1;
        break;
    }
  }

  prevPage(currentIndex?: number) {
    switch (currentIndex) {
      case 1: {
        this.selectedShippingPlans = [];
        break;
      }
    }

    this.stepIndex -= 1;
  }

  onSubmit() {
    this.shippingRequestDetailsControl.clear();

    this.shippingRequestDetails.forEach((i) => {
      this.shippingRequestDetailsControl.push(this.initShippingRequestDetailForm(i));
    });

    this.submitEvent.emit();
  }

  initShippingRequestDetailForm(shippingRequestDetailModel: ShippingRequestDetailModel) {
    return this.fb.group({
      quantity: [shippingRequestDetailModel.quantity],
      productId: [shippingRequestDetailModel.productId],
      amount: [shippingRequestDetailModel.quantity * shippingRequestDetailModel.price],
      price: [shippingRequestDetailModel.price],
      shippingRequestId: [shippingRequestDetailModel.shippingRequestId],
      shippingMode: [shippingRequestDetailModel.shippingMode],
      semlineNumber: [shippingRequestDetailModel.semlineNumber],
      purchaseOrder: [shippingRequestDetailModel.purchaseOrder],
      salesID: [shippingRequestDetailModel.salesID],
      customerName: [shippingRequestDetailModel.customerName],
      accountNumber: [shippingRequestDetailModel.accountNumber],
      productLine: [shippingRequestDetailModel.productLine],
      shippingDate: [Utilities.ConvertDateBeforeSendToServer(shippingRequestDetailModel.shippingDate)],
    });
  }

  _mapToProductsToShippingRequestDetailModels(products: ProductModel[]): ShippingRequestDetailModel[] {
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
        semlineNumber: '',
        purchaseOrder: '',
        salesID: '',
        customerName: '',
        accountNumber: 0,
        productLine: 0,
        shippingDate: null,
      };
    });
  }

  _mapToSelectShippingPlanItem(shippingPlans: ShippingPlanModel[]): SelectItem[] {
    return shippingPlans.map((p) => ({
      value: p,
      label: `${p.identifier} | ${p.purchaseOrder} | ${p.customerName} | ${p.salesID} | ${p.semlineNumber} |
        ${Utilities.ConvertDateBeforeSendToServer(p.shippingDate).toISOString().split('T')[0].split('-').reverse().join('/')}`,
    }));
  }
}
