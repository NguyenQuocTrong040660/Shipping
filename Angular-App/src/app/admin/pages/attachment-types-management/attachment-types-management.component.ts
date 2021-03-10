import { Component, OnInit } from '@angular/core';
import { AttachmentTypeDto, FilesClient } from 'app/shared/api-clients/files.client';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-attachment-types-management',
  templateUrl: './attachment-types-management.component.html',
})
export class AttachmentTypesManagementComponent implements OnInit {
  attachmentTypes: AttachmentTypeDto[] = [];
  cols: any[];
  dialog: boolean;
  attachmentType: AttachmentTypeDto;
  submitted: boolean;
  selectedItems: AttachmentTypeDto[];

  constructor(private filesClient: FilesClient, private messageService: MessageService, private confirmationService: ConfirmationService) {}

  ngOnInit() {
    this.filesClient.apiFilesAttachmenttypeGet().subscribe((data) => {
      this.attachmentTypes = data;
    });

    this.initCols();
  }

  reloadData() {
    this.filesClient.apiFilesAttachmenttypeGet().subscribe((data) => (this.attachmentTypes = data));
  }

  initCols() {
    this.cols = [
      { field: 'id', header: 'Id' },
      { field: 'name', header: 'Type Name' },
      { field: 'created', header: 'Created Date' },
      { field: 'lastModified', header: 'Last Modified Date' },
    ];
  }

  openNew() {
    this.attachmentType = {};
    this.submitted = false;
    this.dialog = true;
  }

  hideDialog() {
    this.dialog = false;
    this.submitted = false;
  }

  editAttachmentType(item: AttachmentTypeDto) {
    this.attachmentType = { ...item };
    this.dialog = true;
  }

  saveAttachmentType() {
    this.submitted = true;

    if (this.attachmentType.name.trim()) {
      !!this.attachmentType.id ? this.updateAttachmentType(this.attachmentType.id, this.attachmentType) : this.addAttachmentType(this.attachmentType);

      this.dialog = false;
      this.attachmentType = {};
    }
  }

  updateAttachmentType(id: string, attachmentType) {
    this.filesClient.apiFilesAttachmenttypePut(id, attachmentType).subscribe((result) => {
      if (result && result.succeeded) {
        this.reloadData();
        this.showMessage('success', 'Successful', 'Attachment Type Updated Successful');
      } else {
        this.showMessage('error', 'Failed', result.error);
      }
    });
  }

  addAttachmentType(attachmentType: AttachmentTypeDto) {
    this.filesClient.apiFilesAttachmenttypePost(attachmentType).subscribe((result) => {
      if (result && result.succeeded) {
        this.showMessage('success', 'Successful', 'Attachment Type Created Successful');
        this.reloadData();
      } else {
        this.showMessage('error', 'Failed', result.error);
      }
    });
  }

  deleteAttachmentType(attachmentType: AttachmentTypeDto) {
    this.confirmationService.confirm({
      message: 'Do you confirm to delete ' + attachmentType.name + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
        this.filesClient
          .apiFilesAttachmenttypeDelete(attachmentType.id)
          .subscribe((result) =>
            result && result.succeeded
              ? this.showMessage('success', 'Successful', 'Attachment Type Successful')
              : this.showMessage('error', 'Failed', 'Delete Attachment Type Failed')
          );
      },
    });
  }

  deleteSelectedItems() {
    this.confirmationService.confirm({
      message: 'Do you confirm to delete the selected items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.products.filter(val => !this.selectedProducts.includes(val));
        // this.selectedProducts = null;
        // this.messageService.add({severity:'success', summary: 'Successful', detail: 'Products Deleted', life: 3000});
      },
    });
  }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
  }
}
