import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CommunicationClient } from 'app/shared/api-clients/communications.client';
import { ShippingPlanClients, ShippingPlanModel, ShippingRequestClients, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import Utilities from 'app/shared/helpers/utilities';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './shipping-request.component.html',
  styleUrls: ['./shipping-request.component.scss'],
})
export class ShippingRequestComponent implements OnInit, OnDestroy {
  title = 'Shipping Request Management';

  shippingRequests: ShippingRequestModel[] = [];
  shippingPlans: ShippingPlanModel[] = [];
  selectedShippingRequest: ShippingRequestModel;

  shippingRequestForm: FormGroup;
  selectItems: SelectItem[] = [];

  isShowDialogCreate: boolean;
  isShowDialogHistory: boolean;
  isShowDialogDocuments: boolean;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];

  expandedItems: any[] = [];

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  selectedShippingPlan: ShippingPlanModel;

  private destroyed$ = new Subject<void>();

  constructor(
    private shippingRequestClients: ShippingRequestClients,
    private shippingPlanClients: ShippingPlanClients,
    private confirmationService: ConfirmationService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
    private communicationClient: CommunicationClient
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Customer Name', field: 'customerName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      // { header: 'Product Line', field: 'productLine', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Bill To', field: 'billTo', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Bill To Address', field: 'billToAddress', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Ship To', field: 'shipTo', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Ship To Address', field: 'shipToAddress', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Account Number', field: 'accountNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Shipping Date', field: 'shippingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Pickup Date', field: 'pickupDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initShippingPlan();
    this.initShippingRequest();
  }

  initShippingPlan() {
    this.shippingPlanClients
      .getAllShippingPlan()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingPlans = i.filter((x) => x.shippingRequestId === null)),
        (_) => (this.shippingPlans = [])
      );
  }

  initForm() {
    this.shippingRequestForm = this.fb.group({
      id: [0],
      notes: [''],
      pickupDate: [new Date()],
      customerName: [''],
      shippingDate: [new Date()],
      billTo: [''],
      billToAddress: [''],
      shipTo: [''],
      shipToAddress: [''],
      shippingPlans: this.fb.array([]),
      lastModifiedBy: [''],
      lastModified: [null],
    });
  }

  initShippingRequest() {
    this.expandedItems = [];

    this.shippingRequestClients
      .getShippingRequests()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingRequests = i),
        (_) => (this.shippingRequests = [])
      );
  }

  openCreateDialog() {
    this.initShippingPlan();
    this.selectedShippingRequest = null;
    this.titleDialog = 'Create Shipping Request';
    this.isShowDialogCreate = true;
  }

  onCreate() {
    if (this.shippingRequestForm.invalid) {
      return;
    }

    const model = this.shippingRequestForm.value as ShippingRequestModel;
    model.id = 0;
    model.shippingDate = Utilities.ConvertDateBeforeSendToServer(model.shippingDate);
    model.pickupDate = Utilities.ConvertDateBeforeSendToServer(model.pickupDate);

    this.shippingRequestClients
      .addShippingRequest(model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (response) => {
          this.sendNotification(response);
          this.notificationService.success('Create Shipping Request Successfully');
          this.initShippingRequest();
          this.hideDialog();
        },
        (_) => this.notificationService.error('Create Shipping Request Failed. Please try again')
      );
  }

  async sendNotification(shippingRequestResponse) {
    try {
      await this.communicationClient.apiCommunicationEmailnotificationShippingRequest(shippingRequestResponse).toPromise();
    } catch (err) {
      console.error(err);
    }
  }

  hideDialog() {
    this.isShowDialogCreate = false;
    this.isShowDialogHistory = false;
    this.isShowDialogDocuments = false;
  }

  openDeleteDialog(singleShippingRequest: ShippingRequestModel) {
    this.confirmationService.confirm({
      message: 'Do you confirm to delete this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingRequestClients
          .deleteShippingRequestAysnc(singleShippingRequest.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.notificationService.success('Delete Shipping Request Successfully');
                this.initShippingRequest();
                this.selectedShippingRequest = null;
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => this.notificationService.error('Delete Shipping Request Failed. Please try again')
          );
      },
    });
  }

  getDetailShippingRequestById(shippingRequestId: number) {
    const shippingRequestSelected = this.shippingRequests.find((i) => i.id === shippingRequestId);

    if (shippingRequestSelected && shippingRequestSelected.shippingPlans && shippingRequestSelected.shippingPlans.length > 0) {
      return;
    }

    this.shippingRequestClients
      .getShippingRequestById(shippingRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ShippingRequestModel) => (shippingRequestSelected.shippingPlans = i.shippingPlans),
        (_) => (shippingRequestSelected.shippingPlans = [])
      );
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  openDocumentsDialog(selectedShippingPlan: ShippingPlanModel) {
    const shippingRequestSelected = this.shippingRequests.find((i) => i.id === selectedShippingPlan.shippingRequestId);
    this.selectedShippingPlan = selectedShippingPlan;
    this.selectedShippingPlan.shippingRequest = shippingRequestSelected;

    this.isShowDialogDocuments = true;
    this.titleDialog = 'Shipping Documents for Sales Order: ' + this.selectedShippingPlan.salesOrder;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
