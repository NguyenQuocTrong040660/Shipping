import { Component, OnInit } from '@angular/core';
import { AlbumService } from '@grx/core';
import { AttachmentTypeDto } from 'app/api-clients/album-client';
import { ConfirmationService, MessageService } from 'primeng/api';
import { company } from '../../../../../assets/company';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductType, Reservation } from 'app/api-clients/shippingapp-client';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ReservationService } from 'app/services/reservation.service';
import { StateService, States } from 'app/services/state.service';
import * as moment from 'moment';
@Component({
  selector: 'app-reservation',
  templateUrl: './reservation.component.html',
  styleUrls: ['./reservation.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class ReservationComponent implements OnInit {
  company = company;
  companyIndexState: number;
  id = '';
  editMode = false;
  form: FormGroup;
  submitted: boolean = false;
  productType: ProductType[] = [];
  reservation: Reservation[] = [];
  titleDialog: string;
  companyDisabled: '';

  get customerName() {
    return this.form.get('customerName');
  }
  get birthDay() {
    return this.form.get('birthDay');
  }
  get dateSet() {
    return this.form.get('dateSet');
  }
  get email() {
    return this.form.get('email');
  }
  get countPerson() {
    return this.form.get('countPerson');
  }
  get phoneNumber() {
    return this.form.get('phoneNumber');
  }
  get serviceName() {
    return this.form.get('serviceName');
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
    private reservationService: ReservationService,
    private stateService: StateService,
  ) {}
  
  ngOnInit() {

    this.reservationService.getReservation().subscribe(data=>{
      this.reservation = data;
    });    

    this.form = this.fb.group({
      customerName: ['', [Validators.required, Validators.maxLength(30)]],
      phoneNumber:['', [Validators.required, Validators.maxLength(12)]],
      birthDay: [null],
      dateSet:[null,[Validators.required]],
      email:[''],
      countPerson:['',[Validators.required, Validators.min(1), Validators.max(10)]],
      serviceName:['', [Validators.required, Validators.maxLength(40)]],
      id: ['00000000-0000-0000-0000-000000000000'],
    });
  }

  reloadData() {
    this.reservationService.getReservation().subscribe((data) => {
     this.reservation = data
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

  editReservation(id: string) {
    this.titleDialog = 'Cập Nhật Thông Tin';
    this.getReservationById(id);

    this.editMode = true;
    this.companyDisabled = '';
    this.dialog = true;
  }


  getReservationById(id) {
    this.reservationService
      .getReservationById(id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((result: Reservation) => {
        if (result) {
          console.log(result);
          const {id, customerName, birthDay, dateSet, email, countPerson, phoneNumber, serviceName } = result;
          this.form.patchValue({
            id: id,
            customerName: customerName,
            birthDay: moment(birthDay).format('YYYY-MM-DD'),
            dateSet: moment(dateSet).format('YYYY-MM-DDTHH:mm:ss'),
            email: email,
            countPerson: countPerson,
            phoneNumber: phoneNumber,
            serviceName: serviceName
          });
        } else {
          this.editMode = false;
          this.id = '00000000-0000-0000-0000-000000000000';
        }
      });
  }


  deleteReservation(reservation: Reservation) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc xóa thông tin của ' + reservation.customerName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
        this.reservationService
          .deleteReservation(reservation.id)
          .subscribe(result =>{
            this.showMessage('success', 'Successful', 'Xóa Thành Công!!!')
            this.reservationService.getReservation().subscribe((data) => {
              this.reservation = data
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
      this.handleUpdateReservation();
    } else {
      //this.handleAddProductType();
    }
  }
  

  handleUpdateReservation() {
    this.reservationService
      .updateReservation(this.form.value.id, this.form.value)
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
