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
  ReceivedMarkSummaryModel,
  RePrintReceivedMarkRequest,
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
  receivedMarks: ReceivedMarkModel[] = [];
  selectedReceivedMark: ReceivedMarkModel;
  movementRequests: MovementRequestModel[] = [];

  receivedMarkMovements: ReceivedMarkMovementModel[] = [];
  receivedMarkPrintings: ReceivedMarkPrintingModel[] = [];
  selectedReceivedMarkPrinting: ReceivedMarkPrintingModel;

  currentReceivedMark: ReceivedMarkModel;
  currentPrintReceivedMarkSummary: ReceivedMarkSummaryModel;
  receivedMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogCreate = false;
  isShowDialogHistory = false;
  isShowDialogUnstuff = false;
  isShowDialogDetail = false;
  canRePrint = false;

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  expandedItems: any[] = [];
  printData: any;

  private destroyed$ = new Subject<void>();

  constructor(
    public printService: PrintService,
    private receivedMarkClients: ReceivedMarkClients,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService,
    private authenticationService: AuthenticationService,
    private fb: FormBuilder,
    private movementRequestClients: MovementRequestClients
  ) {}

  ngOnInit() {
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));

    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Notes', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NumberColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: 'actions', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initReceivedMarks();
    this.intMovementRequest();
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

  intMovementRequest() {
    this.movementRequestClients
      .getMovementRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.movementRequests = i),
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

  hideDialog() {
    this.isShowDialog = false;
    this.isShowDialogCreate = false;
    this.isShowDialogHistory = false;
    this.isShowDialogDetail = false;
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
            this.initReceivedMarks();
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

  openUnstuffDialog(receivedMarkPrintingModel: ReceivedMarkPrintingModel) {
    this.isShowDialogUnstuff = true;
    this.titleDialogUnstufff = `Unstuff #${receivedMarkPrintingModel.sequence} mark`;
    this.selectedReceivedMarkPrinting = receivedMarkPrintingModel;
  }

  handleSubmitEventUnstuff(event) {
    if (!this.currentReceivedMark || !this.currentPrintReceivedMarkSummary) return;

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
            this.notificationService.success('Unstuff Received Mark Successfully');
            this.reLoadReceivedMarkPrintings(this.currentReceivedMark.id, this.currentPrintReceivedMarkSummary.productId);
            this.hideDialogUnStuff();
            return;
          }

          this.notificationService.error(result.error);
        },
        (_) => {
          this.notificationService.error('Unstuff Received Mark Failed');
          this.hideDialogUnStuff();
        }
      );
  }

  printReceivedMark() {
    if (!this.currentReceivedMark || !this.currentPrintReceivedMarkSummary) return;

    const requestPrint: PrintReceivedMarkRequest = {
      productId: this.currentPrintReceivedMarkSummary.productId,
      receivedMarkId: this.currentReceivedMark.id,
      printedBy: this.user.userName,
    };

    this.receivedMarkClients
      .printReceivedMark(requestPrint)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result) {
            this.printData = result;
            this.onPrint();
            this.reLoadReceivedMarkPrintings(this.currentReceivedMark.id, this.currentPrintReceivedMarkSummary.productId);
          } else {
            this.notificationService.error('Print Received Mark Failed. Please try again');
          }
        },
        (_) => this.notificationService.error('Print Received Mark Failed. Please try again')
      );
  }

  handleRePrintMark(item: ReceivedMarkPrintingModel) {
    if (!item) return;

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
            this.onPrint();
            this.reLoadReceivedMarkPrintings(this.currentReceivedMark.id, this.currentPrintReceivedMarkSummary.productId);
          } else {
            this.notificationService.error('RePrint Received Mark Failed. Please try again');
          }
        },
        (_) => this.notificationService.error('RePrint Received Mark Failed. Please try again')
      );
  }

  getReceivedMarkSummaries(item: ReceivedMarkModel) {
    const receivedMark = this.receivedMarks.find((i) => i.id === item.id);

    if (receivedMark && receivedMark.receivedMarkSummaries && receivedMark.receivedMarkSummaries.length > 0) {
      return;
    }

    this.receivedMarkClients
      .getReceivedMarkSummaries(item.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (receivedMark.receivedMarkSummaries = i),
        (_) => (receivedMark.receivedMarkSummaries = [])
      );
  }

  reLoadReceivedMarkPrintings(receivedMarkId: number, productId: number) {
    this.receivedMarkPrintings = [];

    this.receivedMarkClients.getReceivedMarkPrintings(receivedMarkId, productId).subscribe(
      (receivedMarkPrintings) => {
        this.receivedMarkPrintings = receivedMarkPrintings;
      },
      (_) => (this.receivedMarkPrintings = [])
    );
  }

  showDetailReceivedMarkSummary(receivedMark: ReceivedMarkModel, receivedMarkSummaryModel: ReceivedMarkSummaryModel) {
    if (!receivedMark || !receivedMarkSummaryModel) return;

    this.currentReceivedMark = receivedMark;
    this.currentPrintReceivedMarkSummary = receivedMarkSummaryModel;

    this.receivedMarkPrintings = [];
    this.receivedMarkClients.getReceivedMarkPrintings(receivedMark.id, receivedMarkSummaryModel.productId).subscribe(
      (receivedMarkPrintings) => {
        this.receivedMarkPrintings = receivedMarkPrintings;
        this.isShowDialogDetail = true;
        this.titleDialog = `Product Number: ${receivedMarkSummaryModel.product.productNumber} - ${receivedMarkSummaryModel.product.productName}`;
      },
      (_) => this.notificationService.error('Failed to show detail')
    );
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
