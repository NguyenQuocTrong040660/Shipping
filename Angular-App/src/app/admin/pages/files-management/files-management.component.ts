import { Component, OnInit } from '@angular/core';
import { AttachmentDto, AttachmentTypeDto, FilesClient } from 'app/shared/api-clients/files.client';
import { environment } from 'environments/environment';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-files-management',
  templateUrl: './files-management.component.html',
  styleUrls: ['./files-management.component.scss'],
})
export class FilesManagementComponent implements OnInit {
  attachments: AttachmentDto[] = [];
  cols: any[];
  attachmentTypes: AttachmentTypeDto[] = [];
  attachmentType: AttachmentDto = {};

  dialog: boolean;
  uploadDialog: boolean;
  uploadedFiles: any[] = [];

  attachment: AttachmentDto;

  selectedItems: AttachmentDto[];

  constructor(private filesClient: FilesClient, private messageService: MessageService, private confirmationService: ConfirmationService) {}

  ngOnInit() {
    this.initAttachmentTypes();
    this.initDataSource();
    this.initCols();
  }

  initType() {
    if (this.isPhoto()) {
      const attachmentTypeFind = this.attachmentTypes.find((i) => i.name === 'Photo');
      this.attachmentType = attachmentTypeFind === null ? this.attachmentTypes[0] : attachmentTypeFind;
    }

    if (this.isVideo()) {
      const attachmentTypeFind = this.attachmentTypes.find((i) => i.name === 'Video');
      this.attachmentType = attachmentTypeFind === null ? this.attachmentTypes[0] : attachmentTypeFind;
    }
  }

  initAttachmentTypes() {
    this.filesClient.apiFilesAttachmenttypeGet().subscribe((data) => {
      this.attachmentTypes = data;
      this.initType();
    });
  }

  initDataSource() {
    if (this.isPhoto()) {
      this.filesClient.apiFilesAttachmentsPhoto().subscribe((data) => (this.attachments = data));
    }

    if (this.isVideo()) {
      this.filesClient.apiFilesAttachmentsPhoto().subscribe((data) => (this.attachments = data));
    }
  }

  initCols() {
    this.cols = [
      { field: 'id', header: 'Id' },
      { field: 'fileName', header: 'File Name' },
      { field: 'fileUrl', header: 'File Url' },
      { field: 'fileType', header: 'File Type' },
      { field: 'fileSize', header: 'File Size' },
      { field: 'created', header: 'Created Date' },
      { field: 'attachmentTypeName', header: 'Attachment Type' },
    ];
  }

  deleteAttachment(attachment: AttachmentDto) {
    this.confirmationService.confirm({
      message: 'Do you want to delete ' + attachment.fileName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.filesClient.apiFilesAttachmentsDelete(attachment.id).subscribe((result) => {
          if (result && result.succeeded) {
            this.showMessage('success', 'Successful', 'Attachment Deleted Successfuly');
            this.attachments = this.attachments.filter((val) => val.id !== attachment.id);
          } else {
            this.showMessage('error', 'Failed', 'Attachment Delete Failed');
          }
        });
      },
    });
  }

  uploadFile($event, form) {
    const { files } = $event;

    if (files && files.length === 0) {
      return;
    }

    const file = {
      data: files[0],
      fileName: files[0].name,
    };

    this.filesClient.apiFilesAttachmentsPost(file, this.attachmentType.id).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.showMessage('success', 'Successful', 'Create Attachment Successfuly');
          this.initDataSource();
        } else {
          this.showMessage('error', 'Failed', 'Create Attachment Failed');
        }
      },
      (err) => this.showMessage('error', 'Failed', err)
    );

    form.clear();
  }

  getFileUrl(url: string) {
    return `${environment.baseUrl}${url}`;
  }

  getAccessFile() {
    if (this.isPhoto()) {
      return 'image/*';
    }

    if (this.isVideo()) {
      return 'video/mp4,video/x-m4v,video/*';
    }

    return '';
  }

  getMaximumFile() {
    if (this.isPhoto()) {
      return 2097152000;
    }

    if (this.isVideo()) {
      return 20971520000;
    }

    return 0;
  }

  getUrlBulkInsert() {
    if (this.isPhoto()) {
      return `${environment.baseUrl}/api/Attachments/BulkInsertPhotos`;
    }

    if (this.isVideo()) {
      return `${environment.baseUrl}/api/Attachments/BulkInsertVideos`;
    }

    return ``;
  }

  isPhoto() {
    return location.pathname.includes('hinh-anh');
  }

  isVideo() {
    return location.pathname.includes('video');
  }

  formatBytes(bytes: number, decimals = 2) {
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  showDetail(item: AttachmentDto) {
    this.attachment = { ...item };
    this.dialog = true;
  }

  showBulkInsert() {
    this.uploadDialog = true;
  }

  hideDialog() {
    this.dialog = false;
    this.uploadDialog = false;
    this.attachment = {};
  }

  deleteSelectedItems() {
    this.confirmationService.confirm({
      message: 'Do you want to delete the selected items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        const items: string[] = this.selectedItems.map((i) => i.id);

        this.filesClient.apiFilesAttachmentsBulkdeleteattachments(items).subscribe((result) => {
          if (result && result.succeeded) {
            this.showMessage('success', 'Successful', 'Attachments Deleted');
            this.attachments = this.attachments.filter((val) => !this.selectedItems.includes(val));
          } else {
            this.showMessage('error', 'Failed', 'Attachments Delete Failed');
          }
        });
      },
    });
  }

  onUpload(event) {
    for (const file of event.files) {
      this.uploadedFiles = [...this.uploadedFiles, file];
    }

    this.initDataSource();
    this.showMessage('success', 'File Uploaded');
  }

  handleUploadError() {
    this.showMessage('error', 'Upload Failed');
  }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
  }
}
