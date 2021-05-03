import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ProductClients, ProductModel, ShippingPlanClients, ShippingPlanModel } from 'app/shared/api-clients/shipping-app.client';
import { ConfirmationService } from 'primeng/api';
import { NotificationService } from 'app/shared/services/notification.service';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { FilesClient, TemplateType } from 'app/shared/api-clients/files.client';
import { ImportComponent } from 'app/shared/components/import/import.component';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { EventType } from 'app/shared/enumerations/import-event-type.enum';
import { ImportService } from 'app/shared/services/import.service';
import Utilities from 'app/shared/helpers/utilities';

@Component({
  templateUrl: './shipping-plan.component.html',
  styleUrls: ['./shipping-plan.component.scss'],
})
export class ShippingPlanComponent implements OnInit, OnDestroy {
  title = 'Shipping Plan Management';
  titleDialog = '';

  shippingPlans: ShippingPlanModel[] = [];
  selectedShippingPlan: ShippingPlanModel;
  products: ProductModel[] = [];

  shippingPlanForm: FormGroup;

  isShowDialogCreate = false;
  isShowDialogEdit = false;
  isShowDialogHistory = false;

  cols: any[] = [];
  fields: any[] = [];

  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  ref: DynamicDialogRef;

  expandedItems: any[] = [];
  private destroyed$ = new Subject<void>();

  constructor(
    private shippingPlanClients: ShippingPlanClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService,
    private dialogService: DialogService,
    private filesClient: FilesClient,
    private fb: FormBuilder,
    private importService: ImportService
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },

      { header: 'ReferenceId', field: 'refId', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Customer Name', field: 'customerName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Account Number', field: 'accountNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Line', field: 'productLine', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Number', field: 'product', subField: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Sales Order', field: 'salesOrder', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Saleline Number', field: 'salelineNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Purchase Order', field: 'purchaseOrder', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },

      { header: 'Quantity ', field: 'quantity', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Sale Price ', field: 'price', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Shipping Mode ', field: 'shippingMode', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },

      { header: 'Bill To', field: 'billTo', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Bill To Address', field: 'billToAddress', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Ship To', field: 'shipTo', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Ship To Address', field: 'shipToAddress', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },

      { header: 'Shipping Date', field: 'shippingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },

      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initShippingPlans();
    this.initProducts();
    this.initEventBroadCast();
  }

  initEventBroadCast() {
    this.importService.getEvent().subscribe((event) => {
      switch (event) {
        case EventType.HideDialog:
          if (this.ref) {
            this.ref.close();
          }
          this.initShippingPlans();
          break;
      }
    });
  }

  initForm() {
    this.shippingPlanForm = this.fb.group({
      id: [''],
      customerName: ['', [Validators.required]],
      salesOrder: [0, [Validators.required]],
      salelineNumber: [0, [Validators.required]],
      purchaseOrder: ['', [Validators.required]],
      shippingDate: ['', [Validators.required]],

      billTo: ['', [Validators.required]],
      billToAddress: ['', [Validators.required]],
      shipTo: ['', [Validators.required]],
      shipToAddress: ['', [Validators.required]],

      accountNumber: ['', [Validators.required]],
      productLine: ['', [Validators.required]],
      shippingRequestId: [null],

      quantity: [0],
      productId: [0],
      amount: [0],
      price: [0],
      shippingMode: [''],

      lastModifiedBy: [''],
      lastModified: [null],
      notes: [''],
    });
  }

  openImportSection() {
    this.ref = this.dialogService.open(ImportComponent, {
      header: 'IMPORT SHIPPING PLANS',
      width: '100%',
      contentStyle: { height: '800px', overflow: 'auto' },
      baseZIndex: 10000,
      data: TemplateType.ShippingPlan,
    });

    this.ref.onClose.subscribe(() => this.initShippingPlans());
  }

  exportTemplate() {
    this.filesClient.apiFilesExportTemplate(TemplateType.ShippingPlan).subscribe(
      (i) => {
        const aTag = document.createElement('a');
        aTag.id = 'downloadButton';
        aTag.style.display = 'none';
        aTag.href = i;
        aTag.download = 'ProductTemplate';
        document.body.appendChild(aTag);
        aTag.click();
        window.URL.revokeObjectURL(i);

        aTag.remove();
      },
      (_) => this.notificationService.error('Failed to export template')
    );
  }

  initShippingPlans() {
    this.expandedItems = [];

    this.shippingPlanClients
      .getAllShippingPlan()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i) => (this.shippingPlans = i),
        (_) => (this.shippingPlans = [])
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

  openCreateDialog() {
    this.shippingPlanForm.reset();
    this.titleDialog = 'Create Shipping Plan';
    this.isShowDialogCreate = true;
  }

  onCreate() {
    const model = this.shippingPlanForm.value as ShippingPlanModel;
    model.id = 0;
    model.shippingDate = Utilities.ConvertDateBeforeSendToServer(model.shippingDate);

    this.shippingPlanClients.addShippingPlan(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Shipping Plan Successfully');
          this.initShippingPlans();
          this.hideDialog();
        } else {
          this.notificationService.error(result?.error);
        }
      },
      (_) => this.notificationService.error('Create Shipping Plan Failed. Please try again')
    );
  }

  openEditDialog() {
    this.titleDialog = 'Edit Shipping Plan';
    this.shippingPlanForm.patchValue({
      ...this.selectedShippingPlan,
      ...{
        shippingDate: new Date(this.selectedShippingPlan.shippingDate),
      },
    });
    this.isShowDialogEdit = true;
  }

  hideDialog() {
    this.isShowDialogCreate = false;
    this.isShowDialogEdit = false;
    this.isShowDialogHistory = false;
    this.selectedShippingPlan = null;
    this.shippingPlanForm.reset();
  }

  onEdit() {
    let { id, shippingDate } = this.shippingPlanForm.value as ShippingPlanModel;
    shippingDate = Utilities.ConvertDateBeforeSendToServer(shippingDate);

    this.shippingPlanClients
      .updateShippingPlan(id, this.shippingPlanForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Edit Shipping Plan Successfully');
            this.initShippingPlans();
            this.hideDialog();
          } else {
            this.notificationService.error(result?.error);
          }
        },
        (_) => this.notificationService.error('Edit Shipping Plan Failed. Please try again')
      );
  }

  openDeleteDialog(shippingPlan: ShippingPlanModel) {
    this.confirmationService.confirm({
      message: 'Do you confirm to delete this item?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shippingPlanClients
          .deletedShippingPlan(shippingPlan.id)
          .pipe(takeUntil(this.destroyed$))
          .subscribe(
            (result) => {
              if (result && result.succeeded) {
                this.notificationService.success('Delete Shipping Plan Successfully');
                this.selectedShippingPlan = null;
                this.initShippingPlans();
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => this.notificationService.error('Delete Shipping Plan Failed. Please try again')
          );
      },
    });
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();

    if (this.ref) {
      this.ref.close();
    }
  }
}
