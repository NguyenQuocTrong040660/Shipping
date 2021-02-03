import { Component, OnInit } from '@angular/core';
import { AlbumService } from '@grx/core';
import { AttachmentTypeDto } from 'app/api-clients/album-client';
import { ConfirmationService, MessageService } from 'primeng/api';
import { company } from '../../../../../assets/company';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MemberShip } from 'app/api-clients/shippingapp-client';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { MemberShipService } from 'app/services/member-ship.service';
import { StateService, States } from 'app/services/state.service';
import * as moment from 'moment';
@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class MemberShipComponent implements OnInit {
  id = '';
  editMode = false;
  form: FormGroup;
  submitted: boolean = false;
  memberShip: MemberShip[] = [];
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
    private memberShipService: MemberShipService,
    private stateService: StateService,
  ) {}
  
  ngOnInit() {
    this.memberShipService.getMemberShip().subscribe(data=>{
      this.memberShip = data;
    });    

    this.form = this.fb.group({
      customerName: ['', [Validators.required, Validators.maxLength(30)]],
      phoneNumber:['', [Validators.required, Validators.maxLength(12)]],
      birthDay: [null],
      id: ['00000000-0000-0000-0000-000000000000'],
    });
  }

  reloadData() {
    this.memberShipService.getMemberShip().subscribe((data) => {
     this.memberShip = data
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

  editMemberShip(id: string) {
    this.titleDialog = 'Cập Nhật Thông Tin';
    this.getMemberShipById(id);
    this.editMode = true;
    this.companyDisabled = '';
    this.dialog = true;
  }


  getMemberShipById(id) {
    this.memberShipService
      .getMemberShipById(id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((result: MemberShip) => {
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


  deleteMemberShip(MemberShip: MemberShip) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc xóa thông tin của ' + MemberShip.customerName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
        this.memberShipService
          .deleteMemberShip(MemberShip.id)
          .subscribe(result =>{
            this.showMessage('success', 'Successful', 'Xóa Thành Công!!!')
            this.memberShipService.getMemberShip().subscribe((data) => {
              this.memberShip = data
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
      this.handleUpdateMemberShip();
    } else {
      //this.handleAddProductType();
    }
  }

  handleUpdateMemberShip() {
    this.memberShipService
      .updateMemberShip(this.form.value.id, this.form.value)
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
