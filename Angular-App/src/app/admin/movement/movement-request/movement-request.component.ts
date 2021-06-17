import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, SelectItem } from 'primeng/api';
import {
  MovementRequestClients,
  MovementRequestDetailModel,
  MovementRequestModel,
  WorkOrderClients,
  WorkOrderModel,
} from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { NotificationService } from 'app/shared/services/notification.service';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './movement-request.component.html',
  styleUrls: ['./movement-request.component.scss'],
})
export class MovementRequestComponent implements OnInit, OnDestroy {
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
  movementRequestDetails: MovementRequestDetailModel[] = [];

  cols: any[] = [];
  colFields = [];

  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  selectItems: SelectItem[] = [];
  expandedItems: any[] = [];

  private destroyed$ = new Subject<void>();

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
      { header: '....', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
      { header: 'Work Orders', field: 'workOrdersCollection', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.colFields = this.cols.map((i) => i.field);

    this.initMovementRequests();
    this.initWorkOrders();
    this.initForm();
  }

  initWorkOrders() {
    this.workOrderClients
      .getWorkOrders()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.selectItems = this._mapToSelectItems(i)),
        (_) => (this.selectItems = [])
      );
  }

  initMovementRequests() {
    this.expandedItems = [];

    this.movementRequestClients
      .getMovementRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.movementRequests = i),
        (_) => (this.movementRequests = [])
      );
  }

  _mapToSelectItems(workOrders: WorkOrderModel[]): SelectItem[] {
    return workOrders
      .filter((i) => i.canSelected)
      .map((p) => ({
        value: p,
        label: `Work Order Id: ${p.refId} | Product Number: ${p.product?.productNumber}`,
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
      this.movementRequestClients.generateMovementRequests(workOrders).subscribe(
        (movementRequestDetails) => {
          this.movementRequestDetails = movementRequestDetails;
          this.movementRequestDetails.forEach((i, index) => {
            i['id'] = ++index;
            i.quantity = i.workOrder.remainQuantity;
          });
        },
        (_) => (this.movementRequestDetails = [])
      );
    }
  }

  onCreate() {
    const model = this.movementRequestForm.value as MovementRequestModel;
    model.id = 0;

    this.movementRequestClients
      .addMovementRequest(model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Create Movement Request Successfully');
            this.initMovementRequests();
            this.initWorkOrders();
            this.hideDialog();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => this.notificationService.error('Create Movement Request Failed. Please try again')
      );
  }

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

    this.movementRequestClients
      .getMovementRequestById(movementRequest.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: MovementRequestModel) => {
          this.selectedMovementRequest = i;
          this.isShowDialogEdit = true;
        },
        (_) => (this.selectedMovementRequest.movementRequestDetails = [])
      );
  }

  onEdit() {
    const { id } = this.movementRequestForm.value;

    this.movementRequestClients
      .updateMovementRequest(id, this.movementRequestForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Edit Movement Request Successfully');
            this.initMovementRequests();
            this.initWorkOrders();
            this.hideDialog();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => this.notificationService.error('Edit Movement Request Failed. Please try again')
      );
  }

  openDeleteDialog(movementRequest: MovementRequestModel) {
    this.confirmationService.confirm({
      message: 'Do you confirm to delete this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.movementRequestClients
          .deleteMovementRequestAysnc(movementRequest.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.selectedMovementRequest = null;
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

  getDetailMovementRequest(movementRequest: MovementRequestModel) {
    const movementRequests = this.movementRequests.find((i) => i.id === movementRequest.id);

    if (movementRequests && movementRequests.movementRequestDetails && movementRequests.movementRequestDetails.length > 0) {
      return;
    }

    this.movementRequestClients
      .getMovementRequestById(movementRequest.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: MovementRequestModel) => (movementRequests.movementRequestDetails = i.movementRequestDetails),
        (_) => (movementRequests.movementRequestDetails = [])
      );
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}

export interface MovementRequestDetail extends MovementRequestDetailModel {
  isEditRow?: boolean;
  inputQuantity?: number;
}
