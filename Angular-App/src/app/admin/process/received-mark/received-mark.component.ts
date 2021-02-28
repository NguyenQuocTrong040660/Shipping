import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MovementRequestClients, MovementRequestModel, ReceivedMarkClients, ReceivedMarkModel, UnstuffReceivedMarkRequest } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { Roles } from 'app/shared/enumerations/roles.enum';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { SelectItem, ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './received-mark.component.html',
  styleUrls: ['./received-mark.component.scss'],
})
export class ReceivedMarkComponent implements OnInit, OnDestroy {
  title = 'Received Mark';

  user: ApplicationUser;
  receivedMarks: ReceivedMarkModel[] = [];
  selectedReceivedMark: ReceivedMarkModel;
  movementRequests: MovementRequestModel[] = [];

  currentSelectedReceivedMark: ReceivedMarkModel[] = [];
  receivedMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
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
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Sequence', field: 'sequence', width: WidthColumn.QuantityColumn, type: TypeColumn.NumberColumn },
      { header: 'Quantity', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NumberColumn },
      { header: 'Print By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Print Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.intMovementRequest();
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));
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
    this.movementRequestClients
      .getMovementRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.movementRequests = i;
        },
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
          this.updateRowGroupMetaData();
        },
        (_) => (this.receivedMarks = [])
      );
  }

  initReceivedMarksByMovementRequest(momentRequestId: number) {
    this.receivedMarkClients
      .getReceivedMarksByMovementRequestId(momentRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => {
          this.receivedMarks = i;
          this.updateRowGroupMetaData();
        },
        (_) => (this.receivedMarks = [])
      );
  }

  handleOnChange(selectedMovementRequest) {
    this.selectedMovementRequest = selectedMovementRequest;
    this.initReceivedMarksByMovementRequest(selectedMovementRequest);
  }

  openCreateDialog() {
    this.receivedMarkForm.reset();
    this.titleDialog = 'Create Received Mark';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  hideDialog() {
    this.isShowDialog = false;
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

  onSort() {
    this.updateRowGroupMetaData();
  }

  customSort() {
    this.receivedMarks.sort((a, b) => {
      const aGroup = a.product.productNumber.toLowerCase();
      const bGroup = b.product.productNumber.toLowerCase();

      if (aGroup > bGroup) {
        return 1;
      }
      if (aGroup < bGroup) {
        return -1;
      }

      const aSequence = a.sequence;
      const bSequene = b.sequence;

      if (aSequence > bSequene) {
        return 1;
      }
      if (aSequence < bSequene) {
        return -1;
      }
      return 0;
    });
  }

  updateRowGroupMetaData() {
    this.rowGroupMetadata = {};

    if (this.receivedMarks) {
      for (let i = 0; i < this.receivedMarks.length; i++) {
        let rowData = this.receivedMarks[i];
        let representativeName = rowData.product.productNumber;

        if (i == 0) {
          this.rowGroupMetadata[representativeName] = { index: 0, size: 1 };
        } else {
          let previousRowData = this.receivedMarks[i - 1];
          let previousRowGroup = previousRowData.product.productNumber;
          if (representativeName === previousRowGroup) this.rowGroupMetadata[representativeName].size++;
          else this.rowGroupMetadata[representativeName] = { index: i, size: 1 };
        }
      }
    }
  }

  _mapToMovementRequestSelectItems(movementRequests: MovementRequestModel[]): SelectItem[] {
    return movementRequests.map((i) => ({
      value: i.id,
      label: `${i.identifier}`,
    }));
  }

  openUnstuffDialog() {
    this.isShowDialogUnstuff = true;
    this.titleDialog = 'Unstuff Received Mark';
  }

  handleSubmitEventUnstuff(event) {
    const { unstuffQuantity } = event;

    const unstuffRequest: UnstuffReceivedMarkRequest = {
      id: this.selectedReceivedMark.id,
      unstuffQuantity: unstuffQuantity,
    };

    this.receivedMarkClients
      .unstuffReceivedMark(unstuffRequest)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result.succeeded) {
            this.notificationService.success('Unstuff Received Mark Successfully');
            this.initReceivedMarksByMovementRequest(this.selectedMovementRequest);
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
    if (this.user && this.user.roles && this.user.roles.length > 0 && this.user.roles.includes(Roles.Manager)) {
      return '';
    }

    return 'Receive Mark has been printed. Please contact your manager to re-print';
  }

  canRePrint() {
    if (this.user && this.user.roles && this.user.roles.length > 0) {
      return this.user.roles.includes(Roles.Manager);
    }

    return false;
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
