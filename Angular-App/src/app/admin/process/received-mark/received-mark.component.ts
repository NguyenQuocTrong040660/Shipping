import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  MovementRequestClients,
  MovementRequestModel,
  ReceivedMarkClients,
  ReceivedMarkModel,
  ReceivedMarkMovementModel,
  UnstuffReceivedMarkRequest,
} from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { ConfirmationService } from 'primeng/api';
import { forkJoin, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-received-mark',
  templateUrl: './received-mark.component.html',
  styleUrls: ['./received-mark.component.scss'],
})
export class ReceivedMarkComponent implements OnInit, OnDestroy {
  title = 'Received Mark';

  user: ApplicationUser;
  receivedMarks: ReceivedMarkModel[] = [];
  selectedReceivedMark: ReceivedMarkModel;
  movementRequests: MovementRequestModel[] = [];

  receivedMarkMovements: ReceivedMarkMovementModel[] = [];

  currentSelectedReceivedMark: ReceivedMarkModel[] = [];
  receivedMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogCreate = false;
  isShowDialogHistory = false;
  isShowDialogUnstuff = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  selectedMovementRequest = 0;
  rowGroupMetadata: any;

  private destroyed$ = new Subject<void>();

  constructor(
    public printService: PrintService,
    private receivedMarkClients: ReceivedMarkClients,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService,
    private movementRequestClients: MovementRequestClients,
    private authenticationService: AuthenticationService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));

    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Notes', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NumberColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.intMovementRequest();
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

  intMovementRequest() {
    this.movementRequestClients
      .getMovementRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.movementRequests = i),
        (_) => (this.movementRequests = [])
      );
  }

  initReceivedMarks() {
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

  handleSelectedWorkOrders() {
    const { movementRequests } = this.receivedMarkForm.value;

    if (movementRequests && movementRequests.length > 0) {
      forkJoin(movementRequests.map((movementRequestModel: MovementRequestModel) => this.movementRequestClients.getMovementRequestByIdWithoutWO(movementRequestModel.id)))
        .pipe(takeUntil(this.destroyed$))
        .subscribe((results) => {
          results.map((movementRequestModel: MovementRequestModel) => {
            movementRequestModel.movementRequestDetails.map((item) => {
              const receivedMarkMovement: ReceivedMarkMovementModel = {
                product: item.product,
                productId: item.productId,
                movementRequest: movementRequestModel,
                movementRequestId: item.movementRequestId,
                quantity: item.quantity,
                receivedMarkId: 0,
              };

              this.receivedMarkMovements.push(receivedMarkMovement);
            });
          });

          this.receivedMarkMovements.forEach((i, index) => (i['id'] = ++index));
        });
    }
  }

  openCreateDialog() {
    this.receivedMarkForm.reset();
    this.titleDialog = 'Create Received Mark';
    this.isShowDialogCreate = true;
    this.isEdit = false;
  }

  hideDialog() {
    this.isShowDialog = false;
    this.isShowDialogCreate = false;
    this.isShowDialogHistory = false;
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

    this.receivedMarkClients
      .addReceivedMark(model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
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

  openEditDialog(receivedMark: ReceivedMarkModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Edit Received Mark';
    this.isEdit = true;
    this.receivedMarkForm.patchValue(receivedMark);
  }

  openDeleteDialog(receivedMark: ReceivedMarkModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
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

  openUnstuffDialog() {
    this.isShowDialogUnstuff = true;
    this.titleDialog = 'Unstuff Received Mark';
  }

  handleSubmitEventUnstuff(event) {
    const { unstuffQuantity } = event;

    const unstuffRequest: UnstuffReceivedMarkRequest = {
      receivedMarkPrintingId: 0,
      unstuffQuantity: unstuffQuantity,
    };

    this.receivedMarkClients
      .unstuffReceivedMark(unstuffRequest)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result.succeeded) {
            this.notificationService.success('Unstuff Received Mark Successfully');
            this.hideDialog();
            return;
          }

          this.notificationService.error(result.error);
        },
        (_) => this.notificationService.error('Unstuff Received Mark Failed')
      );
  }

  printReceivedMark(receivedMark: ReceivedMarkModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to print mark for this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.receivedMarkClients
          .printReceivedMark(receivedMark.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.onPrint();
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => {
              this.notificationService.error('Print Received Mark Failed. Please try again');
            }
          );
      },
    });
  }

  getHelpText() {
    return this.printService.canRePrint(this.user) ? '' : 'Receive Mark has been printed. Please contact your manager to re-print';
  }

  canRePrint() {
    return this.printService.canRePrint(this.user) ? true : false;
  }

  handleRePrintMark(item: ReceivedMarkModel) {
    if (!this.canRePrint()) {
      return;
    }

    this.confirmationService.confirm({
      message: 'Are you sure you want to re-print this mark ?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.receivedMarkClients
          .printReceivedMark(item.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.onPrint();
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => {
              this.notificationService.error('Print Received Mark Failed. Please try again');
            }
          );
      },
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
