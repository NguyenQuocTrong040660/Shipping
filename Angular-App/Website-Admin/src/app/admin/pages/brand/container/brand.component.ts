import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { company } from './../../../../../../src/assets/company';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Brand } from 'app/api-clients/shippingapp-client';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { BrandService } from 'app/services/brand.service';
import { StateService, States } from 'app/services/state.service';
import { async } from '@angular/core/testing';

@Component({
  selector: 'app-brand',
  templateUrl: './brand.component.html',
  styleUrls: ['./brand.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class BrandComponent implements OnInit {
  company = company;
  companyIndexState: number;
  id = '';
  editMode = false;
  form: FormGroup;
  submitted: boolean = false;
  brand: Brand[] = [];
  titleDialog: string;
  companyDisabled: '';

  get brandName() {
    return this.form.get('brandName');
  }
  get companyIndex() {
    return this.form.get('companyIndex');
  }
  private destroyed$ = new Subject<void>();

  dialog: boolean = false;
  // selectedItems: AttachmentTypeDto[];

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder,
    private serivce: BrandService,
    private stateService: StateService,
  ) {}
  
  ngOnInit() {
    this.stateService.selectState(companyIndex=>{
      this.serivce.getAllBrand(2).subscribe((data)=>{
        this.brand = data;
        this.companyIndexState = 2
      });

    }).subscribe();

    this.form = this.fb.group({
      brandName: ['', [Validators.required]],
      companyIndex: [''],
      id: ['00000000-0000-0000-0000-000000000000'],
    });
  }

  reloadData() {
    this.serivce.getAllBrand(this.companyIndexState).subscribe((data) => {
     this.brand = data
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

  editBrand(id: string) {
    this.titleDialog = 'Cập Nhật Sản Phẩm';
    this.getBrandById(id);
    this.editMode = true;
    this.companyDisabled = '';
    this.dialog = true;
  }


  getBrandById(id) {
    this.serivce
      .getBrandById(id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((result: Brand) => {
        if (result) {
          const {id, brandName, companyIndex} = result;
          this.form.patchValue({
            companyIndex: companyIndex,
            brandName,
            id: id
          });
        } else {
          this.editMode = false;
          this.id = '00000000-0000-0000-0000-000000000000';
        }
      });
  }


   deleteBrand(brand: Brand) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc xóa thương hiệu ' + brand.brandName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
         this.serivce
          .deleteBrand(brand.id)
          .subscribe((result) =>
            this.showMessage('success', 'Successful', 'Xóa Thương Hiệu Thành Công!!!')
          );
          this.serivce.getAllBrand(2).subscribe((data) => {
            this.brand = data
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
      this.handleUpdateBrand();
    } else {
      this.handleAddBrand();
    }
  }

  handleAddBrand() {
    this.form.value.id =  '00000000-0000-0000-0000-000000000000';
    this.form.value.companyIndex = 2 ;
    this.serivce.
      addBrand(this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Thêm mới thương hiệu thành công!!!')
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('error', 'Failed', 'Thêm mới thương hiệu thất bại, Vui lòng kiểm tra lại!!!')
          }
        },
        (e) => {
          this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
        }
      );
  }

  handleUpdateBrand() {
    this.serivce
      .updateBrand(this.form.value.id, this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Cập nhật thương hiệu thành công!!!')
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('success', 'Successful', 'Cập nhật thương hiệu thất bại!!!')
          }
        },
        (e) => {
          this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
        }
      );
  }
}
