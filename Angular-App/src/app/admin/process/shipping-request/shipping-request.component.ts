import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommunicationClient } from 'app/shared/api-clients/communications.client';
import { ProductClients, ProductModel, ShippingPlanClients, ShippingPlanModel, ShippingRequestClients, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import Utilities from 'app/shared/helpers/utilities';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './shipping-request.component.html',
  styleUrls: ['./shipping-request.component.scss'],
})
export class ShippingRequestComponent implements OnInit, OnDestroy {
  title = 'Shipping Request Management';

  shippingRequests: ShippingRequestModel[] = [];
  shippingPlans: ShippingPlanModel[] = [];
  selectedShippingRequest: ShippingRequestModel;
  selectedShippingPlan: ShippingPlanModel;

  products: ProductModel[] = [];

  shippingRequestForm: FormGroup;

  isEdit: boolean;
  isShowDialogCreate: boolean;
  isShowDialogEdit: boolean;
  isShowDialogHistory: boolean;
  isShowDialogDocuments: boolean;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];

  expandedItems: any[] = [];

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  private destroyed$ = new Subject<void>();

  constructor(
    private shippingRequestClients: ShippingRequestClients,
    private shippingPlanClients: ShippingPlanClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService,
    private fb: FormBuilder,
    private communicationClient: CommunicationClient
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Purchase Order', field: 'purchaseOrder', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Customer Name', field: 'customerName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Sales Id', field: 'salesID', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Semline Number', field: 'semlineNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NumberColumn },
      { header: 'Shipping Date', field: 'shippingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initShippingPlan();
    this.initShippingRequest();
    this.initProducts();
  }

  initShippingPlan() {
    this.shippingPlanClients
      .getAllShippingPlan()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingPlans = i),
        (_) => (this.shippingPlans = [])
      );
  }

  initForm() {
    this.shippingRequestForm = this.fb.group({
      id: [0],
      customerName: ['', [Validators.required]],
      salesID: [0, [Validators.required]],
      semlineNumber: [0, [Validators.required]],
      purchaseOrder: ['', [Validators.required]],
      shippingDate: ['', [Validators.required]],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      shippingRequestDetails: this.fb.array([]),
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

  initProducts() {
    this.productClients
      .getProducts()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.products = i),
        (_) => (this.products = [])
      );
  }

  handleSelectedShippingPlanEvent(shippingPlanId) {
    if (shippingPlanId) {
      this.shippingPlanClients
        .getShippingPlanById(shippingPlanId)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (shippingPlan) => (this.selectedShippingPlan = shippingPlan),
          (_) => this.notificationService.error('Failed to retrieve Shipping Plan')
        );
    }

    this.selectedShippingPlan = null;
  }

  openCreateDialog() {
    this.isEdit = false;
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

    this.shippingRequestClients
      .addShippingRequest(model)
      .pipe(takeUntil(this.destroyed$))
      .pipe(switchMap((response) => this.communicationClient.apiCommunicationEmailnotificationShippingRequest(response)))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Create Shipping Request Successfully');
            this.initShippingRequest();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Create Shipping Request Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  openEditDialog(selectedShippingRequest: ShippingRequestModel) {
    this.titleDialog = 'Edit Shipping Request';
    this.isEdit = true;
    this.isShowDialogEdit = true;
    this.shippingRequestForm.patchValue(selectedShippingRequest);
  }

  hideDialog() {
    this.isShowDialogCreate = false;
    this.isShowDialogEdit = false;
    this.isShowDialogHistory = false;
    this.isShowDialogDocuments = false;
    this.shippingRequestForm.reset();
  }

  onEdit() {
    if (this.shippingRequestForm.invalid) {
      return;
    }

    const { id, shippingDate } = this.shippingRequestForm.value;
    this.shippingRequestForm.value.shippingDate = Utilities.ConvertDateBeforeSendToServer(shippingDate);

    this.shippingRequestClients
      .updateShippingRequest(id, this.shippingRequestForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Edit Shipping Request Successfully');
            this.initShippingRequest();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Edit Shipping Request Failed. Please try again');
          this.hideDialog();
        }
      );
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

    if (shippingRequestSelected && shippingRequestSelected.shippingRequestDetails && shippingRequestSelected.shippingRequestDetails.length > 0) {
      return;
    }

    this.shippingRequestClients
      .getShippingRequestById(shippingRequestId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ShippingRequestModel) => (shippingRequestSelected.shippingRequestDetails = i.shippingRequestDetails),
        (_) => (shippingRequestSelected.shippingRequestDetails = [])
      );
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  openDocumentsDialog() {
    if (!this.selectedShippingRequest) return;

    this.isShowDialogDocuments = true;
    this.titleDialog = 'Shipping Request Documents: ' + this.selectedShippingRequest.identifier;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
