import { Component, OnInit } from '@angular/core';
import { AlbumService } from '@grx/core';
import { AttachmentTypeDto } from 'app/api-clients/album-client';
import { ConfirmationService, MessageService } from 'primeng/api';
import { company } from './../../../../../../src/assets/company';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductType } from 'app/api-clients/shippingapp-client';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ProductTypeService } from 'app/services/product-type.service';
import { StateService, States } from 'app/services/state.service';

@Component({
  selector: 'app-product-type',
  templateUrl: './product-type.component.html',
  styleUrls: ['./product-type.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class ProductTypeComponent implements OnInit {
  company = company;
  companyIndexState: number;
  id = '';
  editMode = false;
  form: FormGroup;
  submitted: boolean = false;
  productType: ProductType[] = [];
  titleDialog: string;
  companyDisabled: '';
  get productTypeName() {
    return this.form.get('productTypeName');
  }
  get companyIndex() {
    return this.form.get('companyIndex');
  }
  private destroyed$ = new Subject<void>();


  attachmentTypes: AttachmentTypeDto[] = [];
  cols: any[];
  dialog: boolean = false;
  attachmentType: AttachmentTypeDto;
  selectedItems: AttachmentTypeDto[];

  constructor(
    private services: AlbumService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder,
    private serivce: ProductTypeService,
    private stateService: StateService,
  ) {}
  
  ngOnInit() {
    this.stateService.selectState(companyIndex=>{
      this.serivce.getProductType(2).subscribe((data)=>{
        this.productType = data;
        this.companyIndexState = 2
      });

    }).subscribe();

    this.form = this.fb.group({
      productTypeName: ['', [Validators.required, Validators.maxLength(30)]],
      companyIndex: [''],
      id: ['00000000-0000-0000-0000-000000000000'],
    });
  }

  reloadData() {
    this.serivce.getProductType(2).subscribe((data) => {
     this.productType = data
    });
  }

  openNew() {
    this.form.reset();
    this.submitted = false;
    this.titleDialog = 'Thêm Sản Phẩm';
    this.editMode = false;
    this.companyDisabled = null;
    this.dialog = true;
  }

  hideDialog() {
    this.dialog = false;
    this.submitted = false;
    this.form.reset();
  }

  editProductType(id: string) {
    this.titleDialog = 'Cập Nhật Sản Phẩm';
    //this.getProductTypeById(id);
    this.editMode = true;
    this.companyDisabled = '';
    this.dialog = true;
  }


  // getProductTypeById(id) {
  //   this.serivce
  //     .getProductTypeById(id)
  //     .pipe(takeUntil(this.destroyed$))
  //     .subscribe((result: ProductType) => {
  //       if (result) {
  //         const {id, productTypeName, companyIndex } = result;
  //         this.form.patchValue({
  //           id: id,
  //           companyIndex: companyIndex,
  //           productTypeName: productTypeName,
  //         });
  //       } else {
  //         this.editMode = false;
  //         this.id = '00000000-0000-0000-0000-000000000000';
  //       }
  //     });
  // }


  deleteProductType(productType: ProductType) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + productType.productTypeName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
        this.serivce
          .deleteProductType(productType.id)
          .subscribe((result) =>
            this.showMessage('success', 'Successful', 'Xóa Loại Sản Phẩm Thành Công!!!')
          );
          this.serivce.getProductType(2).subscribe((data) => {
            this.productType = data
           });
      },
    });
  }

  deleteSelectedItems() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the selected items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.products.filter(val => !this.selectedProducts.includes(val));
        // this.selectedProducts = null;
        // this.messageService.add({severity:'success', summary: 'Successful', detail: 'Products Deleted', life: 3000});
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

    if (this.editMode) {
      this.handleUpdateProductType();
    } else {
      this.handleAddProductType();
    }
  }

  handleAddProductType() {
    this.form.value.id =  '00000000-0000-0000-0000-000000000000';
    this.form.value.companyIndex = 2;
    this.serivce.
      postProductType(this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Thêm mới loại sản phẩm thành công!!!')
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('error', 'Failed', 'Thêm mới loại sản phẩm thất bại, Vui lòng kiểm tra lại!!!')
          }
        },
        (e) => {
          this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
        }
      );
  }

  handleUpdateProductType() {
    this.serivce
      .updateProductType(this.form.value.id, this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Cập nhật loại sản phẩm thành công!!!')
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('success', 'Successful', 'Cập nhật loại sản phẩm thất bại!!!')
          }
        },
        (e) => {
          this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
        }
      );
  }

}
