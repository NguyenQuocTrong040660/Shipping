import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ProductClients, ProductModel, ShippingPlanClients, ShippingPlanModel } from 'app/shared/api-clients/shipping-app.client';
import { SelectItem } from 'primeng/api';
import { NotificationService } from 'app/shared/services/notification.service';

@Component({
  templateUrl: './shipping-plan.component.html',
  styleUrls: ['./shipping-plan.component.scss'],
})
export class ShippingPlanComponent implements OnInit {
  shippingPlans: ShippingPlanModel[] = [];
  selectedShippingPlans: ShippingPlanModel[] = [];
  selectItems: SelectItem[] = [];
  products: ProductModel[] = [];

  isShowDeleteDialog: boolean;
  currentSelectedShippingPlan: ShippingPlanModel[] = [];
  isDeleteMany: boolean;
  shippingPlanForm: FormGroup;

  cols: any[] = [];
  fields: any[] = [];

  isEdit = false;
  isShowDialog = false;
  titleDialog = '';

  get name() {
    return this.shippingPlanForm.get('name');
  }

  constructor(private shippingPlanClients: ShippingPlanClients, private productClients: ProductClients, private notificationService: NotificationService) {}

  ngOnInit() {
    this.cols = [
      { header: 'Name', field: 'name' },
      { header: 'Notes', field: 'notes' },
      { header: 'Created', field: 'created' },
      { header: 'Create By', field: 'createBy' },
      { header: 'Last Modified', field: 'lastModified' },
      { header: 'Last Modified By', field: 'lastModifiedBy' },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initDataSource();
    this.initProducts();
  }

  initForm() {
    this.shippingPlanForm = new FormGroup({
      id: new FormControl(0),
      purchaseOrder: new FormControl('', Validators.required),
      customerName: new FormControl('', Validators.required),
      quantityOrder: new FormControl(0, Validators.required),
      salesPrice: new FormControl(0, Validators.required),
      salesID: new FormControl(0, Validators.required),
      semlineNumber: new FormControl(0, Validators.required),
      shippingMode: new FormControl('', Validators.required),
      shippingDate: new FormControl(null),
      notes: new FormControl(''),
      productId: new FormControl(0, Validators.required),
      created: new FormControl(null),
      createBy: new FormControl(''),
      lastModified: new FormControl(null),
      lastModifiedBy: new FormControl(''),
    });
  }

  initDataSource() {
    this.shippingPlanClients.getAllShippingPlan().subscribe(
      (i) => (this.shippingPlans = i),
      (_) => (this.shippingPlans = [])
    );
  }

  initProducts() {
    this.productClients.getProducts().subscribe(
      (i) => {
        this.products = i;
        this.selectItems = this._mapToSelectItem(i);
      },
      (_) => (this.products = [])
    );
  }

  _mapToSelectItem(products: ProductModel[]): SelectItem[] {
    return products.map((p) => ({
      value: p.id,
      label: `${p.productNumber}-${p.productName}`,
    }));
  }

  // Create Shipping Plan
  openCreateDialog() {
    this.shippingPlanForm.reset();
    this.titleDialog = 'Create Shipping Plan';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  onCreate() {
    const model = this.shippingPlanForm.value as ShippingPlanModel;
    model.id = 0;

    this.shippingPlanClients.addShippingPlan(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Shipping Plan Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Shipping Plan Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  onSubmit() {
    if (this.shippingPlanForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  // Edit Shipping Mark
  openEditDialog(shippingPlan: ShippingPlanModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Create Shipping Mark';
    this.isEdit = true;
    this.shippingPlanForm.patchValue(shippingPlan);
  }

  hideDialog() {
    this.isShowDialog = false;
  }

  onEdit() {
    const { id } = this.shippingPlanForm.value;

    this.shippingPlanClients.updateShippingPlan(id, this.shippingPlanForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Shipping Plan Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Edit Shipping Plan Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  // Delete Shipping Plan
  openDeleteDialog(singleShippingPlan?: ShippingPlanModel) {
    this.isShowDeleteDialog = true;
    this.currentSelectedShippingPlan = [];

    if (singleShippingPlan) {
      this.isDeleteMany = false;
      this.currentSelectedShippingPlan.push(singleShippingPlan);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedShippingPlans: ' + this.selectedShippingPlans);
    } else {
      const shippingPlan = this.currentSelectedShippingPlan[0];
      this.shippingPlanClients.deletedShippingPlan(shippingPlan.id).subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Delete Shipping Plan Successfully');
            this.initDataSource();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDeleteDialog();
        },
        (_) => {
          this.notificationService.error('Delete  Shipping Plan Failed. Please try again');
          this.hideDialog();
        }
      );
    }
  }
}
