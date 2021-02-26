import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ProductClients, ProductModel, ShippingPlanClients, ShippingPlanModel } from 'app/shared/api-clients/shipping-app.client';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { NotificationService } from 'app/shared/services/notification.service';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { FilesClient, TemplateType } from 'app/shared/api-clients/files.client';
import { ImportComponent } from 'app/shared/components/import/import.component';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  templateUrl: './shipping-plan.component.html',
  styleUrls: ['./shipping-plan.component.scss'],
})
export class ShippingPlanComponent implements OnInit, OnDestroy {
  title = 'Shipping Plan Management';

  shippingPlans: ShippingPlanModel[] = [];
  selectedShippingPlan: ShippingPlanModel;
  selectItems: SelectItem[] = [];
  products: ProductModel[] = [];

  shippingPlanForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogHistory = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  ref: DynamicDialogRef;

  constructor(
    private shippingPlanClients: ShippingPlanClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService,
    private dialogService: DialogService,
    private filesClient: FilesClient,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Product Number', field: 'product', subField: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Purchase Order', field: 'purchaseOrder', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Qty Order', field: 'quantityOrder', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Customer Name', field: 'customerName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Sales Price', field: 'salesPrice', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Sales Id', field: 'salesID', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Semline Number', field: 'semlineNumber', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Shipping Mode', field: 'shippingMode', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Shipping Date', field: 'shippingDate', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: 'actions', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initShippingPlans();
    this.initProducts();
  }

  initForm() {
    this.shippingPlanForm = this.fb.group({
      id: [''],
      prefix: [''],
      customerName: ['', [Validators.required]],
      salesID: [0, [Validators.required]],
      semlineNumber: [0, [Validators.required]],
      shippingMode: ['', [Validators.required]],
      shippingDate: ['', [Validators.required]],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
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
    this.shippingPlanClients.getAllShippingPlan().subscribe(
      (i) => (this.shippingPlans = i),
      (_) => (this.shippingPlans = [])
    );
  }

  initProducts() {
    this.productClients.getProducts().subscribe(
      (i) => {
        this.products = i;
        this.selectItems = this._mapToSelectItem(i);
      },
      (_) => (this.products = [])
    );
  }

  _mapToSelectItem(products: ProductModel[]): SelectItem[] {
    return products.map((p) => ({
      value: p.id,
      label: `${p.productNumber}-${p.productName}`,
    }));
  }

  // Create Shipping Plan
  openCreateDialog() {
    this.shippingPlanForm.reset();
    this.titleDialog = 'Create Shipping Plan';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  onCreate() {
    const model = this.shippingPlanForm.value as ShippingPlanModel;
    model.id = 0;

    this.shippingPlanClients.addShippingPlan(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Shipping Plan Successfully');
          this.initShippingPlans();
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

    this.hideDialog();
    // TODO: Do this later with api
    // this.isEdit ? this.onEdit() : this.onCreate();
  }

  // Edit Shipping Mark
  openEditDialog(shippingPlan: ShippingPlanModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Create Shipping Plan';
    this.isEdit = true;
    this.shippingPlanForm.patchValue(shippingPlan);
  }

  hideDialog() {
    this.isShowDialog = false;
    this.isShowDialogHistory = false;
  }

  onEdit() {
    const { id } = this.shippingPlanForm.value;

    this.shippingPlanClients.updateShippingPlan(id, this.shippingPlanForm.value).subscribe(
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
        this.shippingPlanClients.deletedShippingPlan(shippingPlan.id).subscribe(
          (result) => {
            if (result && result.succeeded) {
              this.notificationService.success('Delete Shipping Plan Successfully');
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

  getDetailShippingPlan(shippingPlan: ShippingPlanModel) {
    // TODO: show shipping plan Detail
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }

  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
  }
}
