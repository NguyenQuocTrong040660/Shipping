import { ProductClients, ProductModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService } from 'primeng/api';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FilesClient, TemplateType } from 'app/shared/api-clients/files.client';
import { ImportComponent } from 'app/shared/components/import/import.component';

@Component({
  selector: 'app-work-order',
  templateUrl: './work-order.component.html',
  styleUrls: ['./work-order.component.scss'],
})
export class WorkOrderComponent implements OnInit, OnDestroy {
  title = 'Work Orders Management';

  workOrders: WorkOrderModel[] = [];
  products: ProductModel[] = [];
  selectedWorkOrder: WorkOrderModel;

  workOrderForm: FormGroup;

  isEdit = false;
  isShowDialogCreate = false;
  isShowDialogEdit = false;
  isShowDialogHistory = false;
  titleDialog = '';

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  cols: any[] = [];
  fields: any[] = [];

  ref: DynamicDialogRef;

  constructor(
    private workOrderClients: WorkOrderClients,
    private confirmationService: ConfirmationService,
    private productClients: ProductClients,
    private notificationService: NotificationService,
    private fb: FormBuilder,
    private dialogService: DialogService,
    private filesClient: FilesClient
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Reference Id', field: 'refId', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: 'actions', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initWorkOrders();
    this.initProducts();
    this.initWorkOrderForm();
  }

  openImportSection() {
    this.ref = this.dialogService.open(ImportComponent, {
      header: 'IMPORT WORK ORDERS',
      width: '70%',
      contentStyle: { height: '800px', overflow: 'auto' },
      baseZIndex: 10000,
      data: TemplateType.WorkOrder,
    });
  }

  exportTemplate() {
    this.filesClient.apiFilesExportTemplate(TemplateType.WorkOrder).subscribe(
      (i) => {
        const aTag = document.createElement('a');
        aTag.id = 'downloadButton';
        aTag.style.display = 'none';
        aTag.href = i;
        aTag.download = 'WorkOderTemplate';
        document.body.appendChild(aTag);
        aTag.click();
        window.URL.revokeObjectURL(i);

        aTag.remove();
      },
      (_) => this.notificationService.error('Failed to export template')
    );
  }

  initWorkOrderForm() {
    this.workOrderForm = this.fb.group({
      id: [0],
      refId: [''],
      notes: [''],
      lastModifiedBy: [''],
      lastModified: [null],
      workOrderDetails: this.fb.array([]),
    });
  }

  initWorkOrders() {
    this.workOrderClients.getWorkOrders().subscribe(
      (i) => (this.workOrders = i),
      (_) => (this.workOrders = [])
    );
  }

  initProducts() {
    this.productClients.getProducts().subscribe(
      (i) => (this.products = i),
      (_) => (this.products = [])
    );
  }

  getDetailWorkOrder(workOrder: WorkOrderModel) {
    const workOrderSelected = this.workOrders.find((i) => i.id === workOrder.id);

    if (workOrderSelected && workOrderSelected.workOrderDetails && workOrderSelected.workOrderDetails.length > 0) {
      return;
    }

    this.workOrderClients.getWorkOrderById(workOrder.id).subscribe(
      (i: WorkOrderModel) => {
        workOrderSelected.workOrderDetails = i.workOrderDetails;
      },
      (_) => (workOrderSelected.workOrderDetails = [])
    );
  }

  openCreateDialog() {
    this.titleDialog = 'Create Work Order';
    this.isShowDialogCreate = true;
    this.isEdit = false;
  }

  hideDialog() {
    this.isShowDialogCreate = false;
    this.isShowDialogEdit = false;
    this.isShowDialogHistory = false;
    this.workOrderForm.reset();
  }

  onSubmit() {
    if (this.workOrderForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  onCreate() {
    const model = this.workOrderForm.value as WorkOrderModel;
    model.id = 0;

    this.workOrderClients.addWorkOrder(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Work Order Successfully');
          this.initWorkOrders();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Work Order Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  openEditDialog(workOrder: WorkOrderModel) {
    this.titleDialog = 'Edit Work Order';
    this.isEdit = true;

    this.workOrderClients.getWorkOrderById(workOrder.id).subscribe(
      (i: WorkOrderModel) => {
        this.selectedWorkOrder = i;
        this.isShowDialogEdit = true;
      },
      (_) => (this.selectedWorkOrder.workOrderDetails = [])
    );
  }

  onEdit() {
    const { id } = this.workOrderForm.value;

    this.workOrderClients.updateWorkOrder(id, this.workOrderForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Work Order Successfully');
          this.initWorkOrders();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Edit Work Order Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  openDeleteDialog(singleWorkOrder: WorkOrderModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.workOrderClients.deleteWorkOrderAysnc(singleWorkOrder.id).subscribe(
          (result) => {
            if (result && result.succeeded) {
              this.notificationService.success('Delete Work Order Successfully');
              this.initWorkOrders();
            } else {
              this.notificationService.error(result?.error);
            }
          },
          (_) => this.notificationService.error('Delete Work Order Failed. Please try again')
        );
      },
    });
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
