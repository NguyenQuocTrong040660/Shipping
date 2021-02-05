import { Component, OnInit } from '@angular/core';
import { AttachmentTypeDto } from 'app/api-clients/album-client';
import { AlbumService } from 'app/core/services/album.service';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-attachment-types-management',
  templateUrl: './attachment-types-management.component.html',
})
export class AttachmentTypesManagementComponent implements OnInit {
  attachmentTypes: AttachmentTypeDto[] = [];
  cols: any[];
  dialog: boolean = false;
  attachmentType: AttachmentTypeDto;
  submitted: boolean = false;
  selectedItems: AttachmentTypeDto[];

  constructor(
    private services: AlbumService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.services.getAllAttachmentTypes().subscribe((data) => {
      this.attachmentTypes = data;
    });

    this.initCols();
  }

  reloadData() {
    this.services.getAllAttachmentTypes().subscribe((data) => (this.attachmentTypes = data));
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
      !!this.attachmentType.id
        ? this.updateAttachmentType(this.attachmentType.id, this.attachmentType)
        : this.addAttachmentType(this.attachmentType);

      this.dialog = false;
      this.attachmentType = {};
    }
  }

  updateAttachmentType(id: string, attachmentType) {
    this.services.updateAttachmentType(id, attachmentType).subscribe((result) => {
      if (result && result.succeeded) {
        this.reloadData();
        this.showMessage('success', 'Successful', 'Attachment Type Updated Successful');
      } else {
        this.showMessage('error', 'Failed', result.error);
      }
    });
  }

  addAttachmentType(attachmentType: AttachmentTypeDto) {
    this.services.addAttachmentType(attachmentType).subscribe((result) => {
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
      message: 'Are you sure you want to delete ' + attachmentType.name + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
        this.services
          .deleteAttachmentType(attachmentType.id)
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
      message: 'Are you sure you want to delete the selected items?',
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
