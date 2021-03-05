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

  isEdit = false;
  isShowDialog = false;
  isShowDialogHistory = false;

  cols: any[] = [];
  fields: any[] = [];

  WidthColumn = WidthColumn;
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
    this.initShippingPlans();
    this.initProducts();
    this.initEventBroadCast;
  }

  initEventBroadCast() {
    this.importService.event$.pipe(takeUntil(this.destroyed$)).subscribe((event) => {
      switch (event) {
        case EventType.HideDialog:
          this.ref.close();
          this.initShippingPlans();
          break;
      }
    });
  }

  initForm() {
    this.shippingPlanForm = this.fb.group({
      id: [''],
      customerName: ['', [Validators.required]],
      salesID: [0, [Validators.required]],
      semlineNumber: [0, [Validators.required]],
      purchaseOrder: ['', [Validators.required]],
      shippingDate: ['', [Validators.required]],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      shippingPlanDetails: this.fb.array([]),
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

    this.ref.onClose.subscribe(() => this.initProducts());
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
        (i) => {
          this.shippingPlans = i;

          if (this.selectedShippingPlan) {
            this.selectedShippingPlan = this.shippingPlans.find((s) => s.id === this.selectedShippingPlan.id);
          }
        },
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
    this.selectedShippingPlan = null;
    this.shippingPlanForm.reset();
    this.titleDialog = 'Create Shipping Plan';
    this.isShowDialog = true;
    this.isEdit = false;
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
          this.selectedShippingPlan = null;
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Shipping Plan Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  onSubmit() {
    if (this.shippingPlanForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  openEditDialog() {
    this.isShowDialog = true;
    this.titleDialog = 'Edit Shipping Plan';
    this.isEdit = true;
    this.shippingPlanForm.patchValue(this.selectedShippingPlan);
  }

  hideDialog() {
    this.isShowDialog = false;
    this.isShowDialogHistory = false;
    this.isShowDialog = false;
  }

  onEdit() {
    let { id, shippingDate } = this.shippingPlanForm.value;
    this.shippingPlanForm.value.shippingDate = Utilities.ConvertDateBeforeSendToServer(shippingDate);

    this.shippingPlanClients
      .updateShippingPlan(id, this.shippingPlanForm.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notificationService.success('Edit Shipping Plan Successfully');
            this.initShippingPlans();
          } else {
            this.notificationService.error(result?.error);
          }

          this.hideDialog();
        },
        (_) => {
          this.notificationService.error('Edit Shipping Plan Failed. Please try again');
          this.hideDialog();
        }
      );
  }

  openDeleteDialog(shippingPlan: ShippingPlanModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
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
                this.initShippingPlans();
                this.selectedShippingPlan = null;
              } else {
                this.notificationService.error(result?.error);
              }
            },
            (_) => this.notificationService.error('Delete Shipping Plan Failed. Please try again')
          );
      },
    });
  }

  getDetailShippingPlanById(shippingPlanId: number) {
    const shippingPlanSelected = this.shippingPlans.find((i) => i.id === shippingPlanId);
    shippingPlanSelected.shippingPlanDetails = [];

    this.shippingPlanClients
      .getShippingPlanById(shippingPlanId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (i: ShippingPlanModel) => (shippingPlanSelected.shippingPlanDetails = i.shippingPlanDetails),
        (_) => (shippingPlanSelected.shippingPlanDetails = [])
      );
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  onSelectedShippingPlan() {
    if (this.selectedShippingPlan && this.selectedShippingPlan.id) {
      this.shippingPlanClients
        .getShippingPlanById(this.selectedShippingPlan.id)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (i: ShippingPlanModel) => (this.selectedShippingPlan.shippingPlanDetails = i.shippingPlanDetails),
          (_) => (this.selectedShippingPlan.shippingPlanDetails = [])
        );
    }
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();

    if (this.ref) {
      this.ref.close();
    }
  }
}
