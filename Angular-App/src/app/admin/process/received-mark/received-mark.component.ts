import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MovementRequestClients, MovementRequestModel, ReceivedMarkClients, ReceivedMarkModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { SelectItem, ConfirmationService } from 'primeng/api';

@Component({
  templateUrl: './received-mark.component.html',
  styleUrls: ['./received-mark.component.scss'],
})
export class ReceivedMarkComponent implements OnInit {
  title = 'Received Mark';

  receivedMarks: ReceivedMarkModel[] = [];
  selectedReceivedMark: ReceivedMarkModel;
  movementRequests: MovementRequestModel[] = [];
  selectItems: SelectItem[] = [];

  isShowDeleteDialog: boolean;
  currentSelectedReceivedMark: ReceivedMarkModel[] = [];
  isDeleteMany: boolean;
  receivedMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogHistory = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  selectedMovementRequest = 0;

  get workOrderControl() {
    return this.receivedMarkForm.get('workOrderId');
  }

  get notesControl() {
    return this.receivedMarkForm.get('notes');
  }

  constructor(
    public printService: PrintService,
    private receivedMarkClients: ReceivedMarkClients,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService,
    private movementRequestClients: MovementRequestClients
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Sequence', field: 'sequence', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Number', field: 'product', subField: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Quantity', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.intMovementRequest();
  }

  initForm() {
    this.receivedMarkForm = new FormGroup({
      id: new FormControl(0),
      sequence: new FormControl(0),
      quantity: new FormControl(0),
      status: new FormControl(''),
      notes: new FormControl(''),
      printCount: new FormControl(0),
      productId: new FormControl(0),
      movementRequestId: new FormControl(0),
      lastModifiedBy: new FormControl(''),
      lastModified: new FormControl(null),
    });
  }

  intMovementRequest() {
    this.movementRequestClients.getMovementRequests().subscribe(
      (i) => {
        this.movementRequests = i;
        this.selectItems = this._mapToSelectItems(i);
      },
      (_) => (this.movementRequests = [])
    );
  }

  _mapToSelectItems(movementRequests: MovementRequestModel[]): SelectItem[] {
    return movementRequests.map((i) => ({
      value: i.id,
      label: `MM${i.id} - ${i.lastModified}`,
    }));
  }

  initReceivedMarks(momentRequestId: number) {
    this.receivedMarkClients.getReceivedMarksByMovementRequestId(momentRequestId).subscribe(
      (i) => (this.receivedMarks = i),
      (_) => (this.receivedMarks = [])
    );
  }

  handleOnChange(selectedMovementRequest) {
    this.initReceivedMarks(selectedMovementRequest);
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
    this.isShowDialogHistory = false;
  }

  onEdit() {
    const { id } = this.receivedMarkForm.value;

    this.receivedMarkClients.updateReceivedMark(id, this.receivedMarkForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Received Mark Successfully');
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
  openDeleteDialog(receivedMark: ReceivedMarkModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.receivedMarkClients.deleteReceivedMarkAysnc(receivedMark.id).subscribe(
          (result) => {
            if (result && result.succeeded) {
              this.notificationService.success('Delete Received Mark Successfully');
            } else {
              this.notificationService.error(result?.error);
            }
          },
          (_) => {
            this.notificationService.error('Delete Received Mark Failed. Please try again');
          }
        );
      },
    });
  }

  onPrint() {
    this.printService.printDocument('received-mark');
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }
}
