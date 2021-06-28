import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  MovementRequestClients,
  MovementRequestModel,
  PrintReceivedMarkRequest,
  ReceivedMarkClients,
  ReceivedMarkModel,
  ReceivedMarkMovementModel,
  ReceivedMarkPrintingModel,
  RePrintReceivedMarkRequest,
  UnstuffReceivedMarkRequest,
  WorkOrderModel,
} from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { EventType } from 'app/shared/enumerations/import-event-type.enum';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-received-mark',
  templateUrl: './received-mark.component.html',
  styleUrls: ['./received-mark.component.scss'],
})
export class ReceivedMarkComponent implements OnInit, OnDestroy {
  title = 'Received Mark';
  titleDialog = '';
  titleDialogUnstufff = '';

  user: ApplicationUser;
  selectedReceivedMark: ReceivedMarkModel;
  movementRequests: MovementRequestModel[] = [];

  receivedMarks: ReceivedMarkModel[] = [];
  receivedMarkMovements: ReceivedMarkMovementModel[] = [];
  receivedMarkPrintings: ReceivedMarkPrintingModel[] = [];
  selectedReceivedMarkPrinting: ReceivedMarkPrintingModel;
  selectedReceivedMarkPrintingMerged: ReceivedMarkPrintingModel[] = [];

  currentReceivedMark: ReceivedMarkModel;
  currentReceivedMarkMovementModel: ReceivedMarkMovementModel;
  receivedMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogCreate = false;
  isShowDialogHistory = false;
  isShowDialogUnstuff = false;
  isShowDialogDetail = false;
  isShowDialogEdit = false;
  isShowDialogMergeDetail = false;

  canRePrint = false;

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  expandedItems: any[] = [];
  printData: any;

  activeIndex: number = 0;
  colWorkOrders: any[] = [];
  fieldWorkOrders: any[] = [];
  receivedMarkGroupByWorkOrders: WorkOrderModel[] = [];

  private destroyed$ = new Subject<void>();

  constructor(
    public printService: PrintService,
    private receivedMarkClients: ReceivedMarkClients,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService,
    private authenticationService: AuthenticationService,
    private fb: FormBuilder,
    private movementRequestClients: MovementRequestClients
  ) {
    this.initForm();
    this.initReceivedMarks();
    this.intMovementRequest();
    this.initReceivedMarksGroupByWorkOrders();
  }

  ngOnInit() {
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));

    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: '.....', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
      { header: 'Movement Request - Work Order', field: 'workOrdersMovementCollection', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.colWorkOrders = [
      { header: 'Work Order Id', field: 'refId', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },

      { header: 'Product Number', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Description', field: 'productName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },

      { header: 'Qty/ Pkg', field: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Order Qty', field: 'quantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },

      { header: 'Received Qty', field: 'receivedQuantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Total Packages', field: 'totalPackages', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Printed Qty', field: 'printedQuantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },

      { header: '', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);
    this.fieldWorkOrders = this.colWorkOrders.map((i) => i.field);
    this.canRePrint = this.printService.canRePrint(this.user);
  }

  initForm() {
    this.receivedMarkForm = this.fb.group({
      id: [0],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      movementRequests: [[], [Validators.required]],
      receivedMarkMovements: this.fb.array([]),
    });
  }

  initReceivedMarks() {
    this.expandedItems = [];

    this.receivedMarkClients
      .getReceivedMarks()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.receivedMarks = i;
        },
        (_) => (this.receivedMarks = [])
      );
  }

  initReceivedMarksGroupByWorkOrders() {
    this.receivedMarkClients
      .getReceivedMarkGroupByWorkOrders()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.receivedMarkGroupByWorkOrders = i;
          this.getReceivedMarkMovementRequestsFullInfoByWorkOrder();
        },
        (_) => (this.receivedMarkGroupByWorkOrders = [])
      );
  }

  intMovementRequest() {
    this.movementRequestClients
      .getMovementRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.movementRequests = i.filter((x) => !x.isSelectedByReceivedMark)),
        (_) => (this.movementRequests = [])
      );
  }

  handleSelectedMovementRequests() {
    const { movementRequests } = this.receivedMarkForm.value;

    if (movementRequests && movementRequests.length > 0) {
      this.receivedMarkClients.generateReceivedMarkMovements(movementRequests).subscribe(
        (receivedMarkMovements) => {
          this.receivedMarkMovements = receivedMarkMovements;
          this.receivedMarkMovements.forEach((i, index) => (i['id'] = ++index));
        },
        (_) => (this.receivedMarkMovements = [])
      );
    }
  }

  openCreateDialog() {
    this.receivedMarkForm.reset();
    this.titleDialog = 'Create Received Mark';
    this.isShowDialogCreate = true;
    this.isEdit = false;
  }

  hideDialog(eventType = EventType.HideDialog) {
    if (eventType == EventType.RefreshData && this.selectedReceivedMark) {
      this.getReceivedMarkMovementRequestsFullInfo(this.selectedReceivedMark);
    }

    this.isShowDialog = false;
    this.isShowDialogCreate = false;
    this.isShowDialogEdit = false;
    this.isShowDialogHistory = false;
    this.isShowDialogDetail = false;
    this.isShowDialogMergeDetail = false;
  }

  hideDialogUnStuff() {
    this.selectedReceivedMarkPrinting = null;
    this.isShowDialogUnstuff = false;
  }

  onEdit() {
    const { id } = this.receivedMarkForm.value;

    this.receivedMarkClients
      .updateReceivedMark(id, this.receivedMarkForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Edit Received Mark Successfully');
            this.initReceivedMarks();
            this.initReceivedMarksGroupByWorkOrders();
            this.hideDialog();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => this.notificationService.error('Edit Received Mark Failed. Please try again later later')
      );
  }

  onCreate() {
    const model = this.receivedMarkForm.value as ReceivedMarkModel;
    model.id = 0;

    this.receivedMarkClients
      .addReceivedMark(model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Create Received Mark Successfully');
            this.initReceivedMarks();
            this.initReceivedMarksGroupByWorkOrders();
            this.hideDialog();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => this.notificationService.error('Create Received Mark Failed. Please try again later later')
      );
  }

  onSubmit() {
    if (this.receivedMarkForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  openEditDialog(receivedMark: ReceivedMarkModel) {
    this.titleDialog = 'Edit Received Mark';
    this.isEdit = true;

    this.receivedMarkClients
      .getReceivedMarkById(receivedMark.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ReceivedMarkModel) => {
          let movementRequest = null;

          i.receivedMarkMovements.forEach((item, index) => {
            item['id'] = index++;
            movementRequest = item.movementRequest;
          });

          this.currentReceivedMark = i;
          this.isShowDialogEdit = true;

          this.receivedMarkForm.patchValue(i);
          this.receivedMarkForm.get('movementRequests').patchValue(movementRequest);
        },
        (_) => this.notificationService.error('Failed to open Edit Received Mark')
      );
  }

  openDeleteDialog(receivedMark: ReceivedMarkModel) {
    this.confirmationService.confirm({
      message: 'Do you confirm to delete this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.receivedMarkClients
          .deleteReceivedMarkAysnc(receivedMark.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.notificationService.success('Delete Received Mark Successfully');
                this.initReceivedMarks();
                this.initReceivedMarksGroupByWorkOrders();
                this.selectedReceivedMark = null;
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => this.notificationService.error('Delete Received Mark Failed. Please try again later later')
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

  openUnstuffDialog(receivedMarkPrintingModel: ReceivedMarkPrintingModel) {
    this.isShowDialogUnstuff = true;
    this.titleDialogUnstufff = `Unstuff #${receivedMarkPrintingModel.sequence} mark`;
    this.selectedReceivedMarkPrinting = receivedMarkPrintingModel;
  }

  handleSubmitEventUnstuff(event) {
    if (!this.currentReceivedMark || !this.currentReceivedMarkMovementModel) {
      return;
    }

    const { unstuffQuantity, receivedMarkPrintingId } = event;

    const unstuffRequest: UnstuffReceivedMarkRequest = {
      receivedMarkPrintingId: receivedMarkPrintingId,
      unstuffQuantity: unstuffQuantity,
    };

    this.receivedMarkClients
      .unstuffReceivedMark(unstuffRequest)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result.succeeded) {
            this.notificationService.success('Unstuff Received Mark Successfully', '', 1000);
            this.reLoadReceivedMarkPrintings(
              this.currentReceivedMark.id,
              this.currentReceivedMarkMovementModel.workOrderId,
              this.currentReceivedMarkMovementModel.movementRequestId
            );
            this.hideDialogUnStuff();
            return;
          }

          this.notificationService.error(result.error);
        },
        (_) => this.notificationService.error('Unstuff Received Mark Failed')
      );
  }

  printReceivedMark($event: ReceivedMarkPrintingModel) {
    if (!this.currentReceivedMark || !this.currentReceivedMarkMovementModel) {
      return;
    }

    const requestPrint: PrintReceivedMarkRequest = {
      productId: this.currentReceivedMarkMovementModel.productId,
      receivedMarkId: this.currentReceivedMark.id,
      printedBy: this.user.userName,
      movementRequestId: this.currentReceivedMarkMovementModel.movementRequestId,
    };

    if ($event) {
      this.receivedMarkClients
        .printReceivedMarkWithPrintingId($event.id, requestPrint)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (result) => {
            if (result) {
              this.printData = result;
              this.onPrint();
              this.reLoadReceivedMarkPrintings(
                this.currentReceivedMark.id,
                this.currentReceivedMarkMovementModel.workOrderId,
                this.currentReceivedMarkMovementModel.movementRequestId
              );
            } else {
              this.notificationService.error('Print Received Mark Failed. Please try again later');
            }
          },
          (_) => this.notificationService.error('Print Received Mark Failed. Please try again later')
        );
    } else {
      this.receivedMarkClients
        .printReceivedMark(requestPrint)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (result) => {
            if (result) {
              this.printData = result;
              this.onPrint();
              this.reLoadReceivedMarkPrintings(
                this.currentReceivedMark.id,
                this.currentReceivedMarkMovementModel.workOrderId,
                this.currentReceivedMarkMovementModel.movementRequestId
              );
            } else {
              this.notificationService.error('Print Received Mark Failed. Please try again later');
            }
          },
          (_) => this.notificationService.error('Print Received Mark Failed. Please try again later')
        );
    }
  }

  handleRePrintMark(item: ReceivedMarkPrintingModel) {
    if (!item) {
      return;
    }

    const request: RePrintReceivedMarkRequest = {
      receivedMarkPrintingId: item.id,
      rePrintedBy: this.user.userName,
    };

    this.receivedMarkClients
      .rePrintReceivedMark(request)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result) {
            this.printData = result;
            this.onPrint();
            this.reLoadReceivedMarkPrintings(
              this.currentReceivedMark.id,
              this.currentReceivedMarkMovementModel.workOrderId,
              this.currentReceivedMarkMovementModel.movementRequestId
            );
          } else {
            this.notificationService.error('RePrint Received Mark Failed. Please try again later');
          }
        },
        (_) => this.notificationService.error('RePrint Received Mark Failed. Please try again later')
      );
  }

  getReceivedMarkMovementRequestsFullInfo(item: ReceivedMarkModel) {
    this.selectedReceivedMark = item;
    const receivedMark = this.receivedMarks.find((i) => i.id === item.id);

    this.receivedMarkClients
      .getReceivedMarkMovementsFullInfo(item.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (receivedMarkMovements) => {
          receivedMark.receivedMarkMovements.forEach((item: ReceivedMarkMovementModel) => {
            const receivedMarkMovement = receivedMarkMovements.find(
              (i) => i.receivedMarkId === item.receivedMarkId && i.productId === item.productId && i.movementRequestId === item.movementRequestId
            );
            item.product = receivedMarkMovement.product;
            item.totalPackage = receivedMarkMovement.totalPackage;
            item.totalQuantityPrinted = receivedMarkMovement.totalQuantityPrinted;
          });
        },
        (_) => {}
      );
  }

  getReceivedMarkMovementRequestsFullInfoByWorkOrder() {
    this.receivedMarkGroupByWorkOrders.forEach((workOrder: any) => {
      this.receivedMarkClients
        .getReceivedMarkMovementsFullInfoByWorkOrder(workOrder.id)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (receivedMarkMovements) => {
            workOrder.receivedMarkMovements = receivedMarkMovements;
            workOrder.receivedQuantity = receivedMarkMovements.reduce((i, j) => i + j.quantity, 0);
            workOrder.totalPackages = receivedMarkMovements.reduce((i, j) => i + j.totalPackage, 0);
            workOrder.printedQuantity = receivedMarkMovements.reduce((i, j) => i + j.totalQuantityPrinted, 0);
          },
          (_) => {
            workOrder.receivedMarkMovements = [];
          }
        );
    });
  }

  reLoadReceivedMarkPrintings(receivedMarkId: number, workOrderId: number, movementRequestId: number) {
    this.receivedMarkPrintings = [];

    this.receivedMarkClients
      .getReceivedMarkPrintings(receivedMarkId, workOrderId, movementRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (receivedMarkPrintings) => {
          this.receivedMarkPrintings = receivedMarkPrintings;
        },
        (_) => (this.receivedMarkPrintings = [])
      );
  }

  showDetailReceivedMarkSummary(receivedMark: ReceivedMarkModel, receivedMarkMovementModel: ReceivedMarkMovementModel) {
    if (!receivedMark || !receivedMarkMovementModel) {
      return;
    }

    this.currentReceivedMark = receivedMark;
    this.currentReceivedMarkMovementModel = receivedMarkMovementModel;

    this.receivedMarkPrintings = [];
    this.receivedMarkClients
      .getReceivedMarkPrintings(receivedMark.id, receivedMarkMovementModel.workOrderId, receivedMarkMovementModel.movementRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (receivedMarkPrintings) => {
          this.receivedMarkPrintings = receivedMarkPrintings;
          this.isShowDialogDetail = true;
          this.titleDialog = `Product Number: ${receivedMarkMovementModel.product.productNumber} - ${receivedMarkMovementModel.product.productName}`;
        },
        (_) => this.notificationService.error('Failed to show detail')
      );
  }

  showDetailReceivedMarkByWorkOrder(workOrder: WorkOrderModel) {
    this.receivedMarkPrintings = [];
    this.receivedMarkClients
      .getReceivedMarkPrintingsByWorkOrder(workOrder.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (receivedMarkPrintings) => {
          this.receivedMarkPrintings = receivedMarkPrintings;
          this.isShowDialogDetail = true;
          this.titleDialog = `Product Number: ${workOrder.product.productNumber} - ${workOrder.product.productName}`;
        },
        (_) => this.notificationService.error('Failed to show detail')
      );
  }

  showMergeReceivedMark(receivedMark: ReceivedMarkModel, receivedMarkMovementModel: ReceivedMarkMovementModel) {
    if (!receivedMark || !receivedMarkMovementModel) {
      return;
    }

    this.currentReceivedMark = receivedMark;
    this.currentReceivedMarkMovementModel = receivedMarkMovementModel;

    this.receivedMarkPrintings = [];
    this.receivedMarkClients
      .getReceivedMarkPrintings(receivedMark.id, receivedMarkMovementModel.workOrderId, receivedMarkMovementModel.movementRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (receivedMarkPrintings) => {
          this.receivedMarkPrintings = receivedMarkPrintings;
          this.isShowDialogMergeDetail = true;
          this.titleDialog = `Product Number: ${receivedMarkMovementModel.product.productNumber} - ${receivedMarkMovementModel.product.productName}`;
        },
        (_) => this.notificationService.error('Failed to show detail')
      );
  }

  showMergeReceivedMarkByWorkOrder(workOrder: WorkOrderModel) {
    this.receivedMarkPrintings = [];
    this.receivedMarkClients
      .getReceivedMarkPrintingsByWorkOrder(workOrder.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (receivedMarkPrintings) => {
          this.receivedMarkPrintings = receivedMarkPrintings;
          this.isShowDialogMergeDetail = true;
          this.titleDialog = `Product Number: ${workOrder.product.productNumber} - ${workOrder.product.productName}`;
        },
        (_) => this.notificationService.error('Failed to show detail')
      );
  }

  handleMergeReceivedMarks($event: ReceivedMarkPrintingModel[]) {
    this.selectedReceivedMarkPrintingMerged = $event;

    this.receivedMarkClients
      .mergeReceivedMark(this.selectedReceivedMarkPrintingMerged)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Merged Received Marks Successfully');
            this.hideDialog(EventType.RefreshData);
            this.selectedReceivedMarkPrintingMerged = [];
          } else {
            this.notificationService.error(result.error);
          }
        },
        (_) => this.notificationService.error('Merged Received Marks Failed. Please try again later')
      );
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}

export interface ReceivedMarkMovement extends ReceivedMarkMovementModel {
  isEditRow?: boolean;
  inputQuantity?: number;
}
