import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ProductClients, ProductModel, ShippingMarkClients, ShippingMarkModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { NotificationService } from 'app/shared/services/notification.service';
import { SelectItem } from 'primeng/api';

@Component({
  templateUrl: './shipping-mark.component.html',
  styleUrls: ['./shipping-mark.component.scss'],
})
export class ShippingMarkComponent implements OnInit {
  shippingMarks: ShippingMarkModel[] = [];
  products: ProductModel[] = [];
  selectedShippingMarks: ShippingMarkModel[] = [];
  selectItems: SelectItem[] = [];

  isShowDeleteDialog: boolean;
  currentSelectedShippingMark: ShippingMarkModel[] = [];
  isDeleteMany: boolean;
  shippingMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;

  get quantityControl() {
    return this.shippingMarkForm.get('quantity');
  }

  get productControl() {
    return this.shippingMarkForm.get('productId');
  }

  get notesControl() {
    return this.shippingMarkForm.get('notes');
  }

  get customerControl() {
    return this.shippingMarkForm.get('customerId');
  }

  get revisionControl() {
    return this.shippingMarkForm.get('revision');
  }

  get cartonNumberControl() {
    return this.shippingMarkForm.get('cartonNumber');
  }

  constructor(private shippingMarkClients: ShippingMarkClients, private productClients: ProductClients, private notificationService: NotificationService) { }

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Customer', field: 'customerId', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Quantity', field: 'quantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Revision', field: 'revision', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Carton Number', field: 'cartonNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product', field: 'product', subField: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Created', field: 'created', width: WidthColumn.NormalColumn, type: TypeColumn.DateColumn },
      { header: 'Create By', field: 'createBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Last Modified', field: 'lastModified', width: WidthColumn.NormalColumn, type: TypeColumn.DateColumn },
      { header: 'Last Modified By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn }
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initDataSource();
    this.initProducts();
  }

  initForm() {
    this.shippingMarkForm = new FormGroup({
      id: new FormControl(0),
      productId: new FormControl(0, Validators.required),
      notes: new FormControl(''),
      revision: new FormControl('', [Validators.required]),
      quantity: new FormControl(0, [Validators.required, Validators.min(1)]),
      cartonNumber: new FormControl('', [Validators.required]),
      customerId: new FormControl('', [Validators.required]),
      createdBy: new FormControl(''),
      created: new FormControl(null),
      lastModifiedBy: new FormControl(''),
      lastModified: new FormControl(null),
    });
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

  initDataSource() {
    this.shippingMarkClients.getShippingMarks().subscribe(
      (i) => (this.shippingMarks = i),
      (_) => (this.shippingMarks = [])
    );
  }

  // Create Shipping Marks
  openCreateDialog() {
    this.shippingMarkForm.reset();
    this.titleDialog = 'Create Shipping Mark';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  onCreate() {
    const model = this.shippingMarkForm.value as ShippingMarkModel;
    model.id = 0;

    this.shippingMarkClients.addShippingMark(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Shipping Mark Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Shipping Mark Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  onSubmit() {
    if (this.shippingMarkForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  // Edit Shipping Mark
  openEditDialog(shippingMark: ShippingMarkModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Create Shipping Mark';
    this.isEdit = true;
    this.shippingMarkForm.patchValue(shippingMark);
  }

  hideDialog() {
    this.isShowDialog = false;
  }

  onEdit() {
    const { id } = this.shippingMarkForm.value;

    this.shippingMarkClients.updateShippingRequest(id, this.shippingMarkForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Shipping Mark Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Edit Shipping Mark Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  // Delete Shipping Marks
  openDeleteDialog(shippingMark?: ShippingMarkModel) {
    this.isShowDeleteDialog = true;
    this.currentSelectedShippingMark = [];

    if (shippingMark) {
      this.isDeleteMany = false;
      this.currentSelectedShippingMark.push(shippingMark);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.currentSelectedShippingMark.length === 0) {
      return;
    }

    if (this.isDeleteMany) {
      console.log('this.selectedShippingMarks: ' + this.selectedShippingMarks);
    } else {
      const shippingMark = this.currentSelectedShippingMark[0];
      this.shippingMarkClients.deleteShippingMarkAysnc(shippingMark.id).subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Delete Shipping Mark Successfully');
            this.initDataSource();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDeleteDialog();
        },
        (_) => {
          this.notificationService.error('Delete  Shipping Mark Failed. Please try again');
          this.hideDialog();
        }
      );
    }
  }
}
