import { Component, OnInit } from '@angular/core';
import { AlbumService } from '@grx/core';
import { AttachmentTypeDto } from 'app/api-clients/album-client';
import { ConfirmationService, MessageService } from 'primeng/api';
import { company } from '../../../../../assets/company';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Promotion } from 'app/api-clients/shippingapp-client';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { PromotionService } from 'app/services/promotion.service';
import { StateService, States } from 'app/services/state.service';
import * as moment from 'moment';
@Component({
  selector: 'app-Promotion',
  templateUrl: './Promotion.component.html',
  styleUrls: ['./Promotion.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class PromotionComponent implements OnInit {
  id = '';
  editMode = false;
  form: FormGroup;
  submitted: boolean = false;
  Promotion: Promotion[] = [];
  titleDialog: string;
  companyDisabled: '';

  get customerName() {
    return this.form.get('customerName');
  }
  get birthDay() {
    return this.form.get('birthDay');
  }
  get phoneNumber() {
    return this.form.get('phoneNumber');
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
    private PromotionService: PromotionService,
    private stateService: StateService,
  ) {}
  
  ngOnInit() {
    this.PromotionService.getPromotion().subscribe(data=>{
      this.Promotion = data;
    });    

    this.form = this.fb.group({
      customerName: ['', [Validators.required, Validators.maxLength(30)]],
      phoneNumber:['', [Validators.required, Validators.maxLength(12)]],
      birthDay: [null],
      id: ['00000000-0000-0000-0000-000000000000'],
    });
  }

  reloadData() {
    this.PromotionService.getPromotion().subscribe((data) => {
     this.Promotion = data
    });
  }

  openNew() {
    this.form.reset();
    this.submitted = false;
    this.titleDialog = 'Thêm Sản Phẩm';
    this.companyDisabled = null;
    this.editMode = false;
    this.dialog = true;

  }

  hideDialog() {
    this.dialog = false;
    this.submitted = false;
    this.form.reset();
  }

  editPromotion(id: string) {
    this.titleDialog = 'Cập Nhật Thông Tin';
    this.getPromotionById(id);
    this.editMode = true;
    this.companyDisabled = '';
    this.dialog = true;
  }


  getPromotionById(id) {
    this.PromotionService
      .getPromotionById(id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((result: Promotion) => {
        if (result) {
          console.log(result);
          const {id, customerName, birthDay, phoneNumber} = result;
          this.form.patchValue({
            id: id,
            customerName: customerName,
            birthDay: moment(birthDay).format('YYYY-MM-DD'),
            phoneNumber: phoneNumber,
          });
        } else {
          this.editMode = false;
          this.id = '00000000-0000-0000-0000-000000000000';
        }
      });
  }


  deletePromotion(Promotion: Promotion) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc xóa thông tin của ' + Promotion.customerName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
        this.PromotionService
          .deletePromotion(Promotion.id)
          .subscribe(result =>{
            this.showMessage('success', 'Successful', 'Xóa Thành Công!!!')
            this.PromotionService.getPromotion().subscribe((data) => {
              this.Promotion = data
            });
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
      this.handleUpdatePromotion();
    } else {
      //this.handleAddProductType();
    }
  }

  handleUpdatePromotion() {
    this.PromotionService
      .updatePromotion(this.form.value.id, this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Cập nhật thông tin thành công!!!')
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('success', 'Successful', 'Cập nhật thông tin thất bại!!!')
          }
        },
        (e) => {
          this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
        }
      );
  }

}
