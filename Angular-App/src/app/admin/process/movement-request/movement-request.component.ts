import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { MovementRequestClients, MovementRequestDetailModel, MovementRequestModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { NotificationService } from 'app/shared/services/notification.service';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { forkJoin } from 'rxjs';

@Component({
  templateUrl: './movement-request.component.html',
  styleUrls: ['./movement-request.component.scss'],
})
export class MovementRequestComponent implements OnInit {
  title = 'Movement Request';

  movementRequests: MovementRequestModel[] = [];
  selectedMovementRequest: MovementRequestModel;
  movementRequestForm: FormGroup;

  isEdit = false;
  isShowDialogCreate = false;
  isShowDialogEdit = false;
  isShowDialogHistory = false;
  titleDialog = '';

  workOrders: WorkOrderModel[] = [];
  movementRequestDetails: MovementRequestModel[] = [];

  cols: any[] = [];
  colFields = [];

  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  selectItems: SelectItem[] = [];

  constructor(
    private workOrderClients: WorkOrderClients,
    private notificationService: NotificationService,
    private movementRequestClients: MovementRequestClients,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'ID', field: 'id', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: 'actions', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.colFields = this.cols.map((i) => i.field);
    this.title = this.title.toUpperCase();

    this.initMovementRequests();
    this.initWorkOrders();
    this.initForm();
  }

  initWorkOrders() {
    this.workOrderClients.getWorkOrders().subscribe(
      (i) => (this.selectItems = this._mapToSelectItems(i)),
      (_) => (this.selectItems = [])
    );
  }

  initMovementRequests() {
    this.movementRequestClients.getMovementRequests().subscribe(
      (i) => (this.movementRequests = i),
      (_) => (this.movementRequests = [])
    );
  }

  _mapToSelectItems(workOrders: WorkOrderModel[]): SelectItem[] {
    return workOrders.map((p) => ({
      value: p,
      label: `WO${p.id}`,
    }));
  }

  initForm() {
    this.movementRequestForm = this.fb.group({
      id: [0],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      movementRequestDetails: this.fb.array([]),
      workOrders: [[], [Validators.required]],
    });
  }

  handleSelectedWorkOrders() {
    const { workOrders } = this.movementRequestForm.value;

    if (workOrders && workOrders.length > 0) {
      forkJoin(workOrders.map((workOrder: WorkOrderModel) => this.workOrderClients.getWorkOrderById(workOrder.id))).subscribe((results) => {
        results.map((workOder: WorkOrderModel) => {
          workOder.workOrderDetails.map((item) => {
            const movementRequestDetail: MovementRequestDetailModel = {
              product: item.product,
              productId: item.productId,
              workOrder: workOder,
              workOrderId: workOder.id,
              quantity: item.quantity,
              movementRequestId: 0,
            };

            this.movementRequestDetails.push(movementRequestDetail);
          });
        });

        this.movementRequestDetails.forEach((i, index) => (i['id'] = ++index));
      });
    }
  }

  // Create Movement Request
  onCreate() {
    const model = this.movementRequestForm.value as MovementRequestModel;

    this.movementRequestClients.addMovementRequest(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Movement Request Successfully');
          this.initMovementRequests();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Movement Request Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  // Edit Movement Request
  openCreateDialog() {
    this.titleDialog = 'Create Movement Request';
    this.isShowDialogCreate = true;
    this.isEdit = false;
  }

  hideDialog() {
    this.isShowDialogCreate = false;
    this.isShowDialogEdit = false;
    this.isShowDialogHistory = false;
    this.movementRequestForm.reset();
  }

  onSubmit() {
    if (this.movementRequestForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  openEditDialog(movementRequest: MovementRequestModel) {
    this.titleDialog = 'Edit Movement Request';
    this.isEdit = true;

    this.movementRequestClients.getMovementRequestById(movementRequest.id).subscribe(
      (i: MovementRequestModel) => {
        this.selectedMovementRequest = i;
        this.isShowDialogEdit = true;
      },
      (_) => (this.selectedMovementRequest.movementRequestDetails = [])
    );
  }

  onEdit() {
    const { id } = this.movementRequestForm.value;

    this.movementRequestClients.updateMovementRequest(id, this.movementRequestForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Movement Request Successfully');
          this.initMovementRequests();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Edit Movement Request Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  openDeleteDialog(movementRequest: MovementRequestModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this item ?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.movementRequestClients.deleteMovementRequestAysnc(movementRequest.id).subscribe(
          (result) => {
            if (result && result.succeeded) {
              this.notificationService.success('Delete Movement Request Successfully');
              this.initMovementRequests();
            } else {
              this.notificationService.error(result?.error);
            }
          },
          (_) => this.notificationService.error('Delete Movement Request Failed. Please try again')
        );
      },
    });
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }
}
