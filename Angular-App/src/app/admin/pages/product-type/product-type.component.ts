import { Component, OnInit } from '@angular/core';
import { AttachmentTypeDto } from 'app/shared/api-clients/album-client';
import { ConfirmationService, MessageService } from 'primeng/api';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductType, ShippingAppClients } from 'app/shared/api-clients/shippingapp-client';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-product-type',
  templateUrl: './product-type.component.html',
})
export class ProductTypeComponent implements OnInit {
  id = '';
  form: FormGroup;
  editMode: boolean;
  submitted: boolean;
  productType: ProductType[] = [];
  titleDialog: string;
  companyDisabled: '';

  get productTypeName() {
    return this.form.get('productTypeName');
  }

  private destroyed$ = new Subject<void>();

  attachmentTypes: AttachmentTypeDto[] = [];
  cols: any[];
  dialog: boolean;
  attachmentType: AttachmentTypeDto;
  selectedItems: AttachmentTypeDto[];

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder,
    private shippingClient: ShippingAppClients
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      productTypeName: ['', [Validators.required, Validators.maxLength(30)]],
      id: ['00000000-0000-0000-0000-000000000000'],
    });
  }

  reloadData() {
    // this.shippingClient.getAllProductType(2).subscribe((data) => (this.productType = data));
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

  editProductType(id: string) {
    this.titleDialog = 'Cập Nhật Sản Phẩm';
    this.editMode = true;
    this.dialog = true;
  }

  deleteProductType(productType: ProductType) {
    // this.confirmationService.confirm({
    //   message: 'Are you sure you want to delete ' + productType.productTypeName + '?',
    //   header: 'Confirm',
    //   icon: 'pi pi-exclamation-triangle',
    //   accept: () => {
    //     this.shippingClient
    //       .deleteProductType(productType.id)
    //       .subscribe((_) => this.showMessage('success', 'Successful', 'Xóa Loại Sản Phẩm Thành Công!!!'));

    //     this.shippingClient.getAllProductType(2).subscribe((data) => (this.productType = data));
    //   },
    // });
  }

  deleteSelectedItems() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the selected items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {},
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

    if (this.editMode) {
      this.handleUpdateProductType();
    } else {
      this.handleAddProductType();
    }
  }

  handleAddProductType() {
    // this.form.value.id = '00000000-0000-0000-0000-000000000000';

    // this.shippingClient
    //   .productType(this.form.value)
    //   .pipe(takeUntil(this.destroyed$))
    //   .subscribe(
    //     (result) => {
    //       if (result > 0) {
    //         this.showMessage('success', 'Successful', 'Thêm mới loại sản phẩm thành công!!!');
    //         this.hideDialog();
    //         this.reloadData();
    //       } else {
    //         this.showMessage('error', 'Failed', 'Thêm mới loại sản phẩm thất bại, Vui lòng kiểm tra lại!!!');
    //       }
    //     },
    //     (_) => this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
    //   );
  }

  handleUpdateProductType() {
    // this.shippingClient
    //   .updateProductType(this.form.value.id, this.form.value)
    //   .pipe(takeUntil(this.destroyed$))
    //   .subscribe(
    //     (result) => {
    //       if (result > 0) {
    //         this.showMessage('success', 'Successful', 'Cập nhật loại sản phẩm thành công!!!');
    //         this.hideDialog();
    //         this.reloadData();
    //       } else {
    //         this.showMessage('success', 'Successful', 'Cập nhật loại sản phẩm thất bại!!!');
    //       }
    //     },
    //     (_) => this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
    //   );
  }
}
