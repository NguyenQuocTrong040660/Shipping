import { Component, OnInit } from '@angular/core';
import { FilesClient, TemplateType, ValidateDataRequest } from 'app/shared/api-clients/files.client';
import {
  ProductClients,
  ProductModel,
  ShippingPlanClients,
  ShippingPlanImportModel,
  ShippingPlanModel,
  WorkOrderClients,
  WorkOrderImportModel,
} from 'app/shared/api-clients/shipping-app.client';
import { EventType } from 'app/shared/enumerations/import-event-type.enum';
import { ImportService } from 'app/shared/services/import.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { ConfirmationService, MenuItem } from 'primeng/api';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.scss'],
})
export class ImportComponent implements OnInit {
  typeImport: TemplateType;

  stepItems: MenuItem[];
  stepIndex = 0;

  uploadedFiles: any[] = [];

  data: any[] = [];
  dataValidated: any[] = [];
  failedToImportItems: any[] = [];
  itemCompleted = 0;

  TemplateType = TemplateType;

  constructor(
    private filesClient: FilesClient,
    private productClients: ProductClients,
    private workOrderClients: WorkOrderClients,
    private config: DynamicDialogConfig,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService,
    private shippingPlanClients: ShippingPlanClients,
    private importService: ImportService
  ) {
    this.typeImport = this.config.data;
  }

  ngOnInit(): void {
    this.stepItems = [{ label: 'Upload File' }, { label: 'Preview' }, { label: 'Validation' }, { label: 'Results' }];
  }

  uploadHandler(event, form) {
    const { files } = event;

    if (files && files.length === 0) {
      return;
    }

    const file = {
      data: files[0],
      fileName: files[0].name,
    };

    this.filesClient.apiFilesImport(this.typeImport, file).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.data = result.data;
          this.nextPage(this.stepIndex);
        }
      },
      (_) => this.notificationService.error('Failed to update load data')
    );
  }

  handleUploadError(error) {
    this.notificationService.error(error);
  }

  getAccessFile() {
    return '.xlsx, .xls';
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.stepIndex += 1;
        break;
      }
      case 1: {
        this.dataValidated = [];
        this.handleValidationDataImport(this.data, this.typeImport);
        this.stepIndex += 1;
        break;
      }
      case 2: {
        const invalidItems = this.data.filter((i) => i.valid === false);

        if (invalidItems.length > 0) {
          this.confirmationService.confirm({
            message: 'There are some invalid data. Do you want to import them ?',
            header: 'Warning',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
              this.handleImportDataToServer(this.dataValidated, this.typeImport);
              this.stepIndex += 1;
            },
          });
        } else {
          this.handleImportDataToServer(this.dataValidated, this.typeImport);
          this.stepIndex += 1;
        }

        break;
      }
      case 3: {
        this.importService.dispactEvent(EventType.HideDialog);
        break;
      }
    }
  }

  handleValidationDataImport(data: any[], typeImport: TemplateType) {
    const request: ValidateDataRequest = {
      data,
    };

    switch (typeImport) {
      case TemplateType.Product:
        this.filesClient.apiFilesImportValidate(typeImport, request).subscribe((result) => {
          if (result && result.succeeded) {
            const invalidItems = result.data as ProductModel[];

            data.forEach((item: ProductModel) => {
              const invalidItem = invalidItems.find((i) => i.productNumber === item.productNumber);
              item['valid'] = invalidItem ? false : true;
              this.dataValidated.push(item);
            });
          }
        });
        break;
      case TemplateType.WorkOrder:
        this.filesClient.apiFilesImportValidate(typeImport, request).subscribe((result) => {
          if (result && result.succeeded) {
            const invalidItems = result.data;

            data.forEach((item) => {
              const invalidItem = invalidItems.find((i) => i.workOrderId === item.workOrderId);
              item['valid'] = invalidItem ? false : true;
              this.dataValidated.push(item);
            });
          }
        });
        break;
      case TemplateType.ShippingPlan:
        request.data.forEach((i) => (i['key'] = this.createUUID()));

        this.filesClient.apiFilesImportValidate(typeImport, request).subscribe((result) => {
          if (result && result.succeeded) {
            const invalidItems = result.data;

            data.forEach((item) => {
              const invalidItem = invalidItems.find((i) => i.key === item.key);
              item['valid'] = invalidItem ? false : true;
              this.dataValidated.push(item);
            });
          }
        });
        break;
    }
  }

  handleImportDataToServer(data: any[], typeImport: TemplateType) {
    const dataValids = data.filter((i) => i.valid);

    switch (typeImport) {
      case TemplateType.Product:
        const products: ProductModel[] = dataValids.map((i) => {
          return {
            id: 0,
            productName: i.productName,
            productNumber: i.productNumber,
            qtyPerPackage: i.qtyPerPackage,
            notes: i.notes,
          };
        });

        this.productClients.addProductsAll(products).subscribe(
          (invalidProducts) => {
            this.itemCompleted = products.length - invalidProducts.length;
            this.failedToImportItems = invalidProducts;
          },
          (_) => {
            this.notificationService.error('Failed to import data');
            this.failedToImportItems = products;
            this.itemCompleted = 0;
          }
        );
        break;
      case TemplateType.WorkOrder:
        const workOders: WorkOrderImportModel[] = dataValids.map((i) => {
          return {
            workOrderId: i.workOrderId,
            productNumber: i.productNumber,
            quantity: i.quantity,
            notes: i.notes,
          };
        });

        this.workOrderClients.addWorkOrderAll(workOders).subscribe(
          (invalidWorkOrders) => {
            this.itemCompleted = workOders.length - invalidWorkOrders.length;
            this.failedToImportItems = invalidWorkOrders;
          },
          (_) => {
            this.notificationService.error('Failed to import data');
            this.failedToImportItems = workOders;
            this.itemCompleted = 0;
          }
        );

        break;
      case TemplateType.ShippingPlan:
        const shippingPlans: ShippingPlanImportModel[] = dataValids.map((i) => {
          return {
            shippingPlanId: i.shippingPlanId,
            productNumber: i.productNumber,
            quantityOrder: i.quantityOrder,
            notes: i.notes,
            customerName: i.customerName,
            purchaseOrder: i.purchaseOrder,
            salesID: i.salesID,
            semlineNumber: i.semlineNumber,
            salesPrice: i.salesPrice,
            shippingMode: i.shippingMode,
            shippingDate: i.shippingDate,
          };
        });

        this.shippingPlanClients.bulkInsertShippingPlan(shippingPlans).subscribe(
          (invalidShippingPlans) => {
            this.itemCompleted = shippingPlans.length - invalidShippingPlans.length;
            this.failedToImportItems = invalidShippingPlans;
          },
          (_) => {
            this.notificationService.error('Failed to import data');
            this.failedToImportItems = shippingPlans;
            this.itemCompleted = 0;
          }
        );
        break;
    }
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  getMaximumFile() {
    return 2097152000;
  }

  createUUID() {
    let dt = new Date().getTime();
    let uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      let r = (dt + Math.random() * 16) % 16 | 0;
      dt = Math.floor(dt / 16);
      return (c == 'x' ? r : (r & 0x3) | 0x8).toString(16);
    });

    return uuid;
  }
}
