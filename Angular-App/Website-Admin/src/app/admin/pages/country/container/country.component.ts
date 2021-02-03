import { Component, OnInit, OnDestroy } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { StateService, States } from 'app/services/state.service';
import { CountryService } from 'app/services/country.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { Country, ShippingAppClients } from 'app/api-clients/shippingapp-client';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-country',
  templateUrl: './country.component.html',
  styleUrls: ['./country.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class CountryComponent implements OnInit, OnDestroy {
  companyIndex = 0;
  cols: any[];
  dialog = false;
  submitted = false;
  selectedItems: Country[];
  countries: Country[];
  form: FormGroup;
  editMode = false;

  countryCodeToEdit = '';

  get countryCode() {
    return this.form.get('countryCode');
  }

  get countryName() {
    return this.form.get('countryName');
  }

  private destroyed$ = new Subject<void>();
  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private stateService: StateService,
    private service: CountryService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.initCols();
    this.getAllCountry();
    this.initForm();
  }

  initForm() {
    this.form = this.fb.group({
      // id: ['00000000-0000-0000-0000-000000000000'],
      countryName: ['', [Validators.required, Validators.maxLength(450)]],
      countryCode: ['', [Validators.required]],
    });
  }

  onSubmit() {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    // if (this.editMode) {
    //   this.updateAttachmentType();
    // } else {
    this.addCountry();
    // }
  }

  getAllCountry() {
    this.service
      .getAllCountry()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (data) => {
          if (data) {
            this.countries = data;
          }
        },
        (error) => {
          this.countries = [];
          console.error(error);
        }
      );
  }

  reloadData() {
    this.service.getAllCountry().subscribe((data) => {
      this.countries = data;
    });
  }

  initCols() {
    this.cols = [
      { field: 'countryName', header: 'Tên quốc gia' },
      {
        field: 'countryCode',
        header: 'Mã quốc gia',
      },
      { field: 'created', header: 'Ngày tạo' },
    ];
  }

  openNew() {
    this.initForm();
    this.submitted = false;
    this.dialog = true;
    this.editMode = false;
  }

  hideDialog() {
    this.dialog = false;
    this.submitted = false;
    this.initForm();
  }

  openEdit(item) {
    this.editMode = true;
    this.submitted = false;
    this.dialog = true;
    this.form.patchValue(item);
    this.countryCodeToEdit = item.countryCode;
  }

  editCountry() {
    this.service.updateCountry(this.countryCodeToEdit, this.form.value).subscribe((result) => {
      if (result && result === 1) {
        this.showMessage('success', 'Successful', 'Attachment Type Updated Successful');
        this.reloadData();
        this.hideDialog();
      } else {
        this.showMessage('error', 'Failed', 'Lỗi gì đó');
      }
    });
  }

  addCountry() {
    if (this.editMode) {
      this.editCountry();
    } else {
      this.companyIndex = this.stateService.select('companyIndex');

      this.service.addCountry(this.form.value).subscribe((result) => {
        if (result && result === 1) {
          this.showMessage('success', 'Successful', 'Attachment Type Created Successful');
          this.reloadData();
          this.hideDialog();
        } else {
          this.showMessage('error', 'Failed', 'Thất bại');
        }
      });
    }
  }

  deleteCountry(item) {
    this.confirmationService.confirm({
      message: 'Xác nhận xóa ' + item.countryName + '?',
      header: 'Xóa?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.service.deleteCountry(item.countryCode).subscribe((result) => {
          if (result && result === 1) {
            this.showMessage('success', 'Thành công', 'Đã xóa thành công');
            this.reloadData();
          } else {
            this.showMessage('error', 'Thất bại', 'Xóa thất bại');
          }
        });
      },
    });
  }

  handleValueCkChange(value) {
    this.form.patchValue({
      content: value,
    });
  }
  // deleteSelectedItems() {
  //   this.confirmationService.confirm({
  //     message: 'Are you sure you want to delete the selected items?',
  //     header: 'Confirm',
  //     icon: 'pi pi-exclamation-triangle',
  //     accept: () => {
  //       this.
  //       this.selectedProducts = null;
  //       this.messageService.add({severity:'success', summary: 'Successful', detail: 'Products Deleted', life: 3000});
  //     },
  //   });
  // }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
