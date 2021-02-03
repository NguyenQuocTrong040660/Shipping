import { Component, OnInit } from '@angular/core';
import { AttachmentDto, AttachmentTypeDto } from 'app/api-clients/album-client';
import { AlbumService } from 'app/core/services/album.service';
import { environment } from 'environments/environment';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-files-management',
  templateUrl: './files-management.component.html',
  styleUrls: ['./files-management.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class FilesManagementComponent implements OnInit {
  attachments: AttachmentDto[] = [];
  cols: any[];
  attachmentTypes: AttachmentTypeDto[] = [];
  attachmentType: AttachmentDto = {};

  dialog: boolean = false;
  uploadDialog: boolean = false;
  uploadedFiles: any[] = [];

  attachment: AttachmentDto;

  selectedItems: AttachmentDto[];

  constructor(
    private services: AlbumService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.initAttachmentTypes();
    this.initDataSource();
    this.initCols();
  }

  initType() {
    if (this.isPhoto()) {
      const attachmentTypeFind = this.attachmentTypes.find((i) => i.name == 'Photo');
      this.attachmentType = attachmentTypeFind === null ? this.attachmentTypes[0] : attachmentTypeFind;
    }

    if (this.isVideo()) {
      const attachmentTypeFind = this.attachmentTypes.find((i) => i.name == 'Video');
      this.attachmentType = attachmentTypeFind === null ? this.attachmentTypes[0] : attachmentTypeFind;
    }
  }

  initAttachmentTypes() {
    this.services.getAllAttachmentTypes().subscribe((data) => {
      this.attachmentTypes = data;
      this.initType();
    });
  }

  initDataSource() {
    if (this.isPhoto()) {
      this.services.getAllPhotoAttachments().subscribe((data) => (this.attachments = data));
    }

    if (this.isVideo()) {
      this.services.getAllVideoAttachments().subscribe((data) => (this.attachments = data));
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
      message: 'Are you sure you want to delete ' + attachment.fileName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.services.deleteAttachment(attachment.id).subscribe((result) => {
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

    this.services.postAttachment(file, this.attachmentType.id).subscribe(
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

  getFileUrl(url) {
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
    const path = location.pathname;

    return path.includes('hinh-anh');
  }

  isVideo() {
    const path = location.pathname;
    return path.includes('video');
  }

  formatBytes(bytes, decimals = 2) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  showDetail(item) {
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
      message: 'Are you sure you want to delete the selected items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        const items: string[] = this.selectedItems.map((i) => i.id);

        this.services.deleteAttachments(items).subscribe((result) => {
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
    for (let file of event.files) {
      this.uploadedFiles.push(file);
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
