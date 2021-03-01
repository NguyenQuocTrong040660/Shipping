import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel, ShippingPlanDetailModel } from 'app/shared/api-clients/shipping-app.client';
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

  productCols: any[] = [];
  selectedProducts: ProductModel[] = [];

  shippingDetailModels: ShippingPlanDetailModel[] = [];
  clonedshippingDetailModels: { [s: string]: ShippingPlanDetailModel } = {};

  TypeColumn = TypeColumn;

  get customerNameControl() {
    return this.shippingPlanForm.get('customerName');
  }

  get semlineNumberControl() {
    return this.shippingPlanForm.get('semlineNumber');
  }

  get purchaseOrderControl() {
    return this.shippingPlanForm.get('purchaseOrder');
  }

  get shippingDateControl() {
    return this.shippingPlanForm.get('shippingDate');
  }

  get salesIdControl() {
    return this.shippingPlanForm.get('salesID');
  }

  get notesControl() {
    return this.shippingPlanForm.get('notes');
  }

  get shippingPlanDetailsControl() {
    return this.shippingPlanForm.get('shippingPlanDetails') as FormArray;
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.stepItems = [{ label: 'Shipping Plan Info' }, { label: 'Products' }, { label: 'Price' }, { label: 'Complete' }];

    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Name', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];
  }

  hideDialog() {
    this.selectedProducts = [];
    this.shippingDetailModels = [];
    this.stepIndex = 0;
    this.hideDialogEvent.emit();
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 1: {
        this.shippingDetailModels = this._mapToProductsToShippingDetailModels(this.selectedProducts);
        break;
      }
    }
    this.stepIndex += 1;
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  onRowEditInit(shippingDetailModel: ShippingPlanDetailModel) {
    this.clonedshippingDetailModels[shippingDetailModel.productId] = { ...shippingDetailModel };
  }

  onRowDelete(shippingDetailModel: ShippingPlanDetailModel) {
    this.selectedProducts = this.selectedProducts.filter((i) => i.id !== shippingDetailModel.productId);
    this.shippingDetailModels = this.shippingDetailModels.filter((i) => i.productId !== shippingDetailModel.productId);
  }

  onRowEditSave(shippingDetailModel: ShippingPlanDetailModel) {
    const entity = this.shippingDetailModels.find((i) => i.productId === shippingDetailModel.productId);
    entity.quantity = shippingDetailModel.quantity;
    entity.amount = shippingDetailModel.quantity * shippingDetailModel.price;
    delete this.clonedshippingDetailModels[shippingDetailModel.productId];
  }

  onRowEditCancel(shippingDetailModel: ShippingPlanDetailModel, index: number) {
    this.shippingDetailModels[index] = this.clonedshippingDetailModels[shippingDetailModel.productId];
    delete this.clonedshippingDetailModels[shippingDetailModel.productId];
  }

  checkModifiedQuantity(shippingDetailModels: ShippingPlanDetailModel[]) {
    return shippingDetailModels.filter((i) => i.quantity === 0 || i.price === 0 || i.amount === 0).length === 0;
  }

  onSubmit() {
    this.shippingDetailModels.forEach((i) => {
      this.shippingPlanDetailsControl.push(this.initShippingPlanDetailsForm(i));
    });

    this.submitEvent.emit();
  }

  initShippingPlanDetailsForm(shippingPlanDetailModel: ShippingPlanDetailModel) {
    return this.fb.group({
      quantity: [shippingPlanDetailModel.quantity],
      productId: [shippingPlanDetailModel.productId],
      amount: [shippingPlanDetailModel.quantity * shippingPlanDetailModel.price],
      price: [shippingPlanDetailModel.price],
      shippingPlanId: [shippingPlanDetailModel.shippingPlanId],
      shippingMode: [shippingPlanDetailModel.shippingMode],
    });
  }

  _mapToProductsToShippingDetailModels(products: ProductModel[]): ShippingPlanDetailModel[] {
    return products.map((item, index) => {
      return {
        id: index + 1,
        productId: item.id,
        product: item,
        shippingPlan: null,
        shippingPlanId: 0,
        quantity: 0,
        price: 0,
        amount: 0,
      };
    });
  }
}
