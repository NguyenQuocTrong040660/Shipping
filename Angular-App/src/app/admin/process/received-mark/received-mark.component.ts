import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReceivedMarkClients, ReceivedMarkModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { NotificationService } from 'app/shared/services/notification.service';
import { SelectItem } from 'primeng/api';

@Component({
  templateUrl: './received-mark.component.html',
  styleUrls: ['./received-mark.component.scss'],
})
export class ReceivedMarkComponent implements OnInit {
  receivedMarks: ReceivedMarkModel[] = [];
  selectedReceivedMarks: ReceivedMarkModel[] = [];
  workOrders: WorkOrderModel[] = [];
  selectItems: SelectItem[] = [];

  isShowDeleteDialog: boolean;
  currentSelectedReceivedMark: ReceivedMarkModel[] = [];
  isDeleteMany: boolean;
  receivedMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];

  get workOrderControl() {
    return this.receivedMarkForm.get('workOrderId');
  }

  get notesControl() {
    return this.receivedMarkForm.get('notes');
  }

  constructor(private receivedMarkClients: ReceivedMarkClients, private workOrderClients: WorkOrderClients, private notificationService: NotificationService) {}

  ngOnInit() {
    this.cols = [
      { header: 'Work Order', field: 'workOrder', isDate: false, subField: 'id' },
      { header: 'Notes', field: 'notes', isDate: false },
      { header: 'Created', field: 'created', isDate: true },
      { header: 'Create By', field: 'createBy', isDate: false },
      { header: 'Last Modified', field: 'lastModified', isDate: true },
      { header: 'Last Modified By', field: 'lastModifiedBy', isDate: false },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initDateSource();
    this.initWorkOrders();
  }

  initForm() {
    this.receivedMarkForm = new FormGroup({
      id: new FormControl(0),
      workOrderId: new FormControl(0, Validators.required),
      notes: new FormControl(''),
      createdBy: new FormControl(''),
      created: new FormControl(null),
      lastModifiedBy: new FormControl(''),
      lastModified: new FormControl(null),
    });
  }

  initWorkOrders() {
    this.workOrderClients.getWorkOrders().subscribe(
      (i) => {
        this.workOrders = i;
        this.selectItems = this._mapToSelectItem(i);
      },
      (_) => (this.workOrders = [])
    );
  }

  _mapToSelectItem(workOrders: WorkOrderModel[]): SelectItem[] {
    return workOrders.map((p) => ({
      value: p.id,
      label: `WorkOrder-${p.id}`,
    }));
  }

  initDateSource() {
    this.receivedMarkClients.getReceivedMarks().subscribe(
      (i) => (this.receivedMarks = i),
      (_) => (this.receivedMarks = [])
    );
  }

  // Create Received Marks
  openCreateDialog() {
    this.receivedMarkForm.reset();
    this.titleDialog = 'Create Received Mark';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  hideDialog() {
    this.isShowDialog = false;
  }

  onEdit() {
    const { id } = this.receivedMarkForm.value;

    this.receivedMarkClients.updateReceivedMark(id, this.receivedMarkForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Received Mark Successfully');
          this.initDateSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Edit Received Mark Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  onCreate() {
    const model = this.receivedMarkForm.value as ReceivedMarkModel;
    model.id = 0;

    this.receivedMarkClients.addReceivedMark(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Received Mark Successfully');
          this.initDateSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Received Mark Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  onSubmit() {
    if (this.receivedMarkForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  // Edit Received Mark
  openEditDialog(receivedMark: ReceivedMarkModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Edit Received Mark';
    this.isEdit = true;
    this.receivedMarkForm.patchValue(receivedMark);
  }

  // Delete Received Marks
  openDeleteDialog(receivedMark?: ReceivedMarkModel) {
    this.isShowDeleteDialog = true;
    this.currentSelectedReceivedMark = [];

    if (receivedMark) {
      this.isDeleteMany = false;
      this.currentSelectedReceivedMark.push(receivedMark);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedReceivedMarks: ' + this.selectedReceivedMarks);
    } else {
      const receivedMark = this.currentSelectedReceivedMark[0];
      this.receivedMarkClients.deleteReceivedMarkAysnc(receivedMark.id).subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Delete Received Mark Successfully');
            this.initDateSource();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDeleteDialog();
        },
        (_) => {
          this.notificationService.error('Delete  Received Mark Failed. Please try again');
          this.hideDialog();
        }
      );
    }

    this.hideDeleteDialog();
  }
}
