import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ProductModel, ShippingPlanClients, ShippingPlanDetailModel, ShippingPlanModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-shipping-plan-create',
  templateUrl: './shipping-plan-create.component.html',
  styleUrls: ['./shipping-plan-create.component.scss'],
})
export class ShippingPlanCreateComponent implements OnInit, OnChanges {
  @Input() shippingPlanForm: FormGroup;
  @Input() titleDialog: string;
  @Input() isShowDialog: boolean;
  @Input() products: ProductModel[] = [];
  @Input() selectedShippingPlan: ShippingPlanModel;
  @Input() isEdit: boolean;

  @Output() submitEvent = new EventEmitter<FormGroup>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  stepItems: MenuItem[];
  stepIndex = 0;

  productCols: any[] = [];
  selectedProducts: ProductModel[] = [];

  shippingDetailModels: ShippingPlanDetails[] = [];
  clonedshippingDetailModels: { [s: string]: ShippingPlanDetails } = {};

  TypeColumn = TypeColumn;
  private destroyed$ = new Subject<void>();

  get customerNameControl() {
    return this.shippingPlanForm.get('customerName');
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

  get salesOrderControl() {
    return this.shippingPlanForm.get('salesOrder');
  }

  get notesControl() {
    return this.shippingPlanForm.get('notes');
  }

  get shippingPlanDetailsControl() {
    return this.shippingPlanForm.get('shippingPlanDetails') as FormArray;
  }

  constructor(private fb: FormBuilder, private shippingPlanClients: ShippingPlanClients) {}

  ngOnInit(): void {
    this.stepItems = [{ label: 'Shipping Plan Info' }, { label: 'Products' }, { label: 'Details' }, { label: 'Complete' }];

    this.productCols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Description', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Per Package', field: 'qtyPerPackage', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];
  }

  ngOnChanges() {
    if (this.isEdit) {
      const { customerName, salelineNumber, purchaseOrder, shippingDate, salesOrder, notes } = this.selectedShippingPlan;

      this.customerNameControl.patchValue(customerName);
      this.salelineNumberControl.patchValue(salelineNumber);
      this.purchaseOrderControl.patchValue(purchaseOrder);
      this.salesOrderControl.patchValue(salesOrder);
      this.notesControl.patchValue(notes);
      this.shippingDateControl.patchValue(new Date(shippingDate));
    }
  }

  hideDialog() {
    this.selectedProducts = [];
    this.shippingDetailModels = [];
    this.stepIndex = 0;
    this.hideDialogEvent.emit();
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        if (this.isEdit) {
          this._getDetailShippingPlan();
        }
        break;
      }
      case 1: {
        if (this.isEdit) {
          this._getDetailShippingPlan();
        }

        this.shippingDetailModels = this._mapToProductsToShippingDetailModels(this.selectedProducts);

        if (this.selectedShippingPlan) {
          const { shippingPlanDetails } = this.selectedShippingPlan;

          this.shippingDetailModels.forEach((item) => {
            const shippingPlanDetail = shippingPlanDetails.find((i) => item.productId === i.productId);

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

  onRowEditInit(shippingDetailModel: ShippingPlanDetails) {
    shippingDetailModel.isEditRow = true;
    this.clonedshippingDetailModels[shippingDetailModel.productId] = { ...shippingDetailModel };
  }

  onRowDelete(shippingDetailModel: ShippingPlanDetails) {
    this.selectedProducts = this.selectedProducts.filter((i) => i.id !== shippingDetailModel.productId);
    this.shippingDetailModels = this.shippingDetailModels.filter((i) => i.productId !== shippingDetailModel.productId);
  }

  onRowEditSave(shippingDetailModel: ShippingPlanDetails) {
    shippingDetailModel.isEditRow = false;
    const entity = this.shippingDetailModels.find((i) => i.productId === shippingDetailModel.productId);
    entity.quantity = shippingDetailModel.quantity;
    entity.amount = shippingDetailModel.quantity * shippingDetailModel.price;
    delete this.clonedshippingDetailModels[shippingDetailModel.productId];
  }

  onRowEditCancel(shippingDetailModel: ShippingPlanDetails, index: number) {
    this.shippingDetailModels[index] = this.clonedshippingDetailModels[shippingDetailModel.productId];
    this.shippingDetailModels[index].isEditRow = false;
    delete this.clonedshippingDetailModels[shippingDetailModel.productId];
  }

  allowMoveToCompleteStep(shippingDetailModels: ShippingPlanDetails[]): boolean {
    const haveFilledDataRows = shippingDetailModels.filter((i) => i.quantity === 0 || i.price === 0 || i.amount === 0).length === 0;
    const haveNotEditRows = shippingDetailModels.every((d) => d.isEditRow === false);

    return haveFilledDataRows && haveNotEditRows && this.shippingDetailModels.length > 0;
  }

  onSubmit() {
    this.shippingPlanDetailsControl.clear();

    if (this.shippingDetailModels && this.shippingDetailModels.length > 0) {
      this.shippingDetailModels.forEach((i) => {
        this.shippingPlanDetailsControl.push(this.initShippingPlanDetailsForm(i));
      });
    }

    this.submitEvent.emit();
  }

  initShippingPlanDetailsForm(shippingPlanDetailModel: ShippingPlanDetails) {
    return this.fb.group({
      quantity: [shippingPlanDetailModel.quantity],
      productId: [shippingPlanDetailModel.productId],
      amount: [shippingPlanDetailModel.quantity * shippingPlanDetailModel.price],
      price: [shippingPlanDetailModel.price],
      shippingPlanId: [shippingPlanDetailModel.shippingPlanId],
      shippingMode: [shippingPlanDetailModel.shippingMode],
    });
  }

  _mapToProductsToShippingDetailModels(products: ProductModel[]): ShippingPlanDetails[] {
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
        isEditRow: false,
      };
    });
  }

  _getDetailShippingPlan() {
    const { id } = this.selectedShippingPlan;
    let { shippingPlanDetails } = this.selectedShippingPlan;
    shippingPlanDetails = [];

    this.shippingPlanClients
      .getShippingPlanById(id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ShippingPlanModel) => {
          shippingPlanDetails = i.shippingPlanDetails;
          const products = shippingPlanDetails.map((p) => p.product);
          this.selectedProducts = products;
        },
        (_) => (shippingPlanDetails = [])
      );
  }
}

export interface ShippingPlanDetails extends ShippingPlanDetailModel {
  isEditRow?: boolean;
}
