import { Component, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { ProductClients, ProductModel } from 'app/shared/api-clients/shipping-app.client';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { NotificationService } from 'app/shared/services/notification.service';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
})
export class ProductComponent implements OnInit {
  title = 'Products Management';

  selectedItem: ProductModel;
  products: ProductModel[] = [];

  editMode = false;
  productForm: FormGroup;

  titleDialog: string;
  dialog = false;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  cols: any[] = [];
  fields: any[] = [];

  get productNameControl() {
    return this.productForm.get('productName');
  }

  get productNumberControl() {
    return this.productForm.get('productNumber');
  }

  get notesControl() {
    return this.productForm.get('notes');
  }

  get qtyPerPackageControl() {
    return this.productForm.get('qtyPerPackage');
  }

  private destroyed$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'ID', field: 'id', width: WidthColumn.IdentityColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Name', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty/ Pkg', field: 'qtyPerPackage', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.fields = this.cols.map((i) => i.field);
    this.title = this.title.toUpperCase();
    this.initForm();
    this.initProducts();
  }

  initForm() {
    this.productForm = this.fb.group({
      id: [0],
      productName: ['', [Validators.required]],
      productNumber: ['', [Validators.required]],
      notes: [''],
      qtyPerPackage: ['', [Validators.required, Validators.min(0)]],
      lastModifiedBy: [''],
      lastModified: [null],
    });
  }

  initProducts() {
    this.productClients
      .getProducts()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (data) => (this.products = data),
        (_) => (this.products = [])
      );
  }

  openCreateDialog() {
    this.productForm.reset();
    this.titleDialog = 'Add Product';
    this.editMode = false;
    this.dialog = true;
  }

  hideDialog() {
    this.dialog = false;
    this.productForm.reset();
  }

  editProduct(product: ProductModel) {
    this.titleDialog = 'Edit Product';
    this.productForm.patchValue(product);
    this.editMode = true;
    this.dialog = true;
  }

  deleteProduct(product: ProductModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.productClients
          .deleteProductAysnc(product.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.notificationService.success('Delete Product Successfully');
                this.initProducts();
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => this.notificationService.error('Delete Product Failed. Please try again')
          );
      },
    });
  }

  onSubmit() {
    if (this.productForm.invalid) {
      return;
    }

    this.editMode ? this.handleUpdateProduct() : this.handleAddProduct();
  }

  handleAddProduct() {
    const model = this.productForm.value as ProductModel;
    model.id = 0;

    this.productClients
      .addProducts(model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Add Product Successfully');
            this.initProducts();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Add Product Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  handleUpdateProduct() {
    this.productClients
      .updateProduct(this.productForm.value.id, this.productForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Update Product Successfully');
            this.initProducts();
          } else {
            this.notificationService.error(result?.error);
          }
          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Update Product Failed. Please try again');
          this.hideDialog();
        }
      );
  }
}
