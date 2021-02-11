import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { CountryClients, CountryModel, ProductClients, ProductModel } from 'app/shared/api-clients/shipping-app.client';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
})
export class ProductComponent implements OnInit {
  editMode = false;
  submitted = false;
  form: FormGroup;
  products: ProductModel[] = [];
  country: CountryModel[] = [];

  titleDialog: string;
  dialog = false;

  get productName() {
    return this.form.get('productName');
  }

  get productNumber() {
    return this.form.get('productNumber');
  }

  get notes() {
    return this.form.get('notes');
  }

  get qtyPerPackage() {
    return this.form.get('qtyPerPackage');
  }

  private destroyed$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private countryClients: CountryClients
  ) {}

  ngOnInit() {
    //this.getCountry();
    this.initForm();
  }

  initForm() {
    this.form = this.fb.group({
      id: ['00000000-0000-0000-0000-000000000000'],
      productName: ['', [Validators.required, Validators.maxLength(100)]],
      productNumber: ['', [Validators.required, Validators.maxLength(100)]],
      notes: ['', [Validators.required, Validators.maxLength(100)]],
      qtyPerPackage: ['', [Validators.required, Validators.maxLength(100)]],
    });
  }

  getCountry() {
    this.countryClients
      .getCoutries()
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data) => (this.country = data));
  }

  reloadData() {
    this.productClients.getProducts().subscribe((data) => (this.products = data));
  }

  openNew() {
    this.form.reset();
    this.submitted = false;
    this.titleDialog = 'Thêm Sản Phẩm';
    this.editMode = false;
    this.dialog = true;
  }

  hideDialog() {
    this.dialog = false;
    this.submitted = false;
    this.form.reset();
  }

  editProduct(id: string) {
    this.titleDialog = 'Cập Nhật Sản Phẩm';
    this.getProductById(id);
    this.editMode = true;
    this.dialog = true;
  }

  getProductById(productId) {
    this.productClients
      .getProductById(productId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((result: ProductModel) => {
        if (result) {
          const { id, productName } = result;

          this.form.patchValue({
            id: id,
            productName,
          });
        } else {
          this.editMode = false;
        }
      });
  }

  deleteProduct(product: ProductModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + product.productName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.productClients.deleteProductAysnc(product.id).subscribe((_) => this.showMessage('success', 'Successful', 'Xóa Sản Phẩm Thành Công!!!'));
        this.reloadData();
      },
    });
  }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
  }

  onSubmit() {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    this.editMode ? this.handleUpdateProduct() : this.handleAddProduct();
  }

  handleAddProduct() {
    this.productClients
      .addProducts(this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Thêm mới sản phẩm thành công!!!');
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('error', 'Failed', 'Thêm mới sản phẩm thất bại, Vui lòng kiểm tra lại!!!');
          }
        },
        (_) => this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
      );
  }

  handleUpdateProduct() {
    this.productClients
      .updateProduct(this.form.value.id, this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Cập nhật sản phẩm thành công!!!');
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('success', 'Successful', 'Cập nhật sản phẩm thất bại!!!');
          }
        },
        (_) => this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
      );
  }
}
