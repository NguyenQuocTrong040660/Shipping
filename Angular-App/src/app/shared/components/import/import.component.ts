import { Component, OnInit } from '@angular/core';
import { FilesClient, TemplateType, ValidateDataRequest } from 'app/shared/api-clients/files.client';
import { ProductClients, ProductModel, WorkOrderClients, WorkOrderImportModel, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { NotificationService } from 'app/shared/services/notification.service';
import { MenuItem } from 'primeng/api';
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
  successfullyImportItems: any[] = [];

  TemplateType = TemplateType;

  constructor(
    private filesClient: FilesClient,
    private productClients: ProductClients,
    private workOrderClients: WorkOrderClients,
    private config: DynamicDialogConfig,
    private notificationService: NotificationService
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
        break;
      }
      case 1: {
        this.handleValidationDataImport(this.data, this.typeImport);
        break;
      }
      case 2: {
        this.handleImportDataToServer(this.dataValidated, this.typeImport);
        break;
      }
    }

    this.stepIndex += 1;
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
      default:
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
          };
        });

        this.productClients.addProductsAll(products).subscribe(
          (invalidProducts) => {
            if (invalidProducts && invalidProducts.length > 0) {
              this.failedToImportItems = invalidProducts;
            } else {
              this.successfullyImportItems = products;
            }
          },
          (_) => {}
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
            if (invalidWorkOrders && invalidWorkOrders.length > 0) {
              this.failedToImportItems = invalidWorkOrders;
            } else {
              this.successfullyImportItems = workOders;
            }
          },
          (_) => {}
        );

        break;
      default:
        break;
    }
  }

  prevPage() {
    this.stepIndex -= 1;
  }

  getMaximumFile() {
    return 2097152000;
  }
}
