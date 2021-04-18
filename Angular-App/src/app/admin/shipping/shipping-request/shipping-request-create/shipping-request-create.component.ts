import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
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
export class ShippingRequestCreateComponent implements OnInit, OnChanges {
  @Input() shippingRequestForm: FormGroup;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;
  @Input() shippingPlans: ShippingPlanModel[] = [];

  selectItems: SelectItem[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  products: ProductModel[] = [];
  selectedShippingPlans: ShippingPlanModel[] = [];

  stepItems: MenuItem[];

  minDate = new Date();

  stepIndex = 0;

  selectedProducts: ProductModel[] = [];
  shippingRequestDetails: ShippingRequestDetailModel[] = [];

  TypeColumn = TypeColumn;
  productCols: any[] = [];
  productFields: any[] = [];

  get shippingRequestDetailsControl() {
    return this.shippingRequestForm.get('shippingRequestDetails') as FormArray;
  }

  selecteShippingInfoItems: SelectItem[] = [];
  selectedShippingInfo: any;

  constructor(private fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.shippingPlans && changes.shippingPlans.currentValue) {
      this.selecteShippingInfoItems = this._mapDataToShippingInfoItems(changes.shippingPlans.currentValue);
    }
  }

  ngOnInit(): void {
    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Description', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.productFields = this.productCols.map((i) => i.field);

    this.stepItems = [
      {
        label: 'Shipping Informations',
      },
      {
        label: 'Shipping Plans',
      },
      {
        label: 'Products',
      },
      {
        label: 'Shipping Request Summary',
      },
    ];
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
        const { key, infos } = this.selectedShippingInfo;

        this.shippingRequestForm.patchValue({
          shippingDate: Utilities.ConvertDateBeforeSendToServer(infos.shippingDate),
          billTo: infos.billTo,
          billToAddress: infos.billToAddress,
          shipTo: infos.shipTo,
          shipToAddress: infos.shipToAddress,
          customerName: infos.customerName,
        });

        const shippingPlansFilter = this.shippingPlans.filter((i) => {
          return `${new Date(i.shippingDate).toLocaleDateString()}-${i.billTo}-${i.billToAddress}-${i.shipTo}-${i.shipToAddress}}`.toUpperCase() === key;
        });

        this.selectItems = this._mapToSelectShippingPlanItem(shippingPlansFilter);

        this.stepIndex += 1;
        break;
      }

      case 1: {
        if (!this.selectedShippingPlans || this.selectedShippingPlans.length === 0) {
          return;
        }

        const products = this.selectedShippingPlans.map((i) => i.product);

        this.shippingRequestForm.patchValue({
          pickupDate: Utilities.ConvertDateBeforeSendToServer(this.shippingRequestForm.get('pickupDate').value),
        });

        this.selectedProducts = products;

        this.stepIndex += 1;
        break;
      }
      case 2: {
        this.shippingRequestDetails = this._mapToProductsToShippingRequestDetailModels(this.selectedProducts, this.selectedShippingPlans);
        this.stepIndex += 1;
        break;
      }

      case 3:
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
      salelineNumber: [shippingRequestDetailModel.salelineNumber],
      purchaseOrder: [shippingRequestDetailModel.purchaseOrder],
      salesOrder: [shippingRequestDetailModel.salesOrder],
      productLine: [shippingRequestDetailModel.productLine],
    });
  }

  _mapDataToShippingInfoItems(shippingPlans: ShippingPlanModel[]): SelectItem[] {
    const shippingInfos = shippingPlans.map((i) => {
      return {
        key: `${new Date(i.shippingDate).toLocaleDateString()}-${i.billTo}-${i.billToAddress}-${i.shipTo}-${i.shipToAddress}}`.toUpperCase(),
        infos: {
          shippingDate: i.shippingDate,
          billTo: i.billTo,
          billToAddress: i.billToAddress,
          shipTo: i.shipTo,
          shipToAddress: i.shipToAddress,
          customerName: i.customerName,
          saleOrder: i.salesOrder,
        },
      };
    });

    const filterShippingInfos = shippingInfos.reduce((acc, current) => {
      const x = acc.find((item) => item.key === current.key);
      if (!x) {
        return acc.concat([current]);
      } else {
        return acc;
      }
    }, []);

    return filterShippingInfos.map((p) => ({
      value: p,
      label: `Sale Order: ${p.infos.saleOrder} | Shipping Date: ${Utilities.ConvertDateBeforeSendToServer(p.infos.shippingDate)
        .toISOString()
        .split('T')[0]
        .split('-')
        .reverse()
        .join('/')}`,
    }));
  }

  _mapToSelectShippingPlanItem(shippingPlans: ShippingPlanModel[]): SelectItem[] {
    return shippingPlans.map((p) => ({
      value: p,
      label: `Sale Order: ${p.salesOrder} | Saleline Number: ${p.salelineNumber} | Product Number: ${p.product?.productNumber} | Shipping Date: 
        ${Utilities.ConvertDateBeforeSendToServer(p.shippingDate).toISOString().split('T')[0].split('-').reverse().join('/')}`,
    }));
  }

  _mapToProductsToShippingRequestDetailModels(products: ProductModel[], shippingPlans: ShippingPlanModel[]): ShippingRequestDetailModel[] {
    return shippingPlans.map((item, index) => {
      return {
        id: index + 1,
        productId: item.product.id,
        product: products.find((i) => i.id == item.product.id),
        shippingRequest: null,
        shippingRequestId: 0,
        quantity: item.shippingPlanDetail.quantity,
        price: item.shippingPlanDetail.price,
        amount: item.shippingPlanDetail.amount,
        shippingMode: item.shippingPlanDetail.shippingMode,
        salelineNumber: item.salelineNumber,
        purchaseOrder: item.purchaseOrder,
        salesOrder: item.salesOrder,
        productLine: item.productLine,
      };
    });
  }
}
