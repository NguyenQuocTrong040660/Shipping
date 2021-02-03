import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { VideoHomePageDto } from 'app/api-clients/album-client';
import { AlbumService } from 'app/core/services/album.service';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-youtube-embed-managements',
  templateUrl: './youtube-embed-managements.component.html',
  styleUrls: ['./youtube-embed-managements.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class YoutubeEmbedManagementsComponent implements OnInit {
  videoHomePages: VideoHomePageDto[] = [];
  cols: any[];

  dialog: boolean = false;
  videoHomePage: VideoHomePageDto;
  submitted: boolean = false;

  selectedItems: VideoHomePageDto[];

  constructor(
    private services: AlbumService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit() {
    this.initDataSource();
    this.initCols();
  }

  getLinkIframeYoutube(youtubeId) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${youtubeId}`);
  }

  initDataSource() {
    this.services.getAllVideoHomePages().subscribe((data) => (this.videoHomePages = data));
  }

  initCols() {
    this.cols = [
      { field: 'id', header: 'Id' },
      { field: 'youtubeImage', header: 'youtubeImage' },
      { field: 'youtubeLink', header: 'youtubeLink' },
      { field: 'descriptions', header: 'descriptions' },
      { field: 'width', header: 'width' },
      { field: 'height', header: 'height' },
    ];
  }

  openNew() {
    this.videoHomePage = {};
    this.submitted = false;
    this.dialog = true;
  }

  hideDialog() {
    this.dialog = false;
    this.submitted = false;
  }

  editVideoHomePage(item: VideoHomePageDto) {
    this.videoHomePage = { ...item };
    this.dialog = true;
  }

  saveVideoHomePage() {
    this.submitted = true;

    if (this.videoHomePage.youtubeLink.trim()) {
      !!this.videoHomePage.id
        ? this.updateVideoHomePage(this.videoHomePage.id, this.videoHomePage)
        : this.addVideoHomePage(this.videoHomePage);

      this.dialog = false;
      this.videoHomePage = {};
    }
  }

  updateVideoHomePage(key: string, videoHomePage: VideoHomePageDto) {
    const { id, width, height, youtubeLink, youtubeId, youtubeImage, descriptions, code, file } = videoHomePage;

    this.services
      .updateVideoHomePage(key, id, width, height, youtubeLink, youtubeId, youtubeImage, descriptions, code, null)
      .subscribe((result) => {
        if (result && result.succeeded) {
          this.showMessage('success', 'Successful', 'Attachment Type Updated');
          this.initDataSource();
        } else {
          this.showMessage('error', 'Failed', result.error);
        }
      });
  }

  addVideoHomePage(videoHomePage: VideoHomePageDto) {
    const { id, width, height, youtubeLink, youtubeId, youtubeImage, descriptions, code, file } = videoHomePage;

    this.services
      .addVideoHomePage(id, width, height, youtubeLink, youtubeId, youtubeImage, descriptions, code, null)
      .subscribe((result) => {
        if (result && result.succeeded) {
          this.showMessage('success', 'Successful', 'Attachment Type Created');
          this.initDataSource();
        } else {
          this.showMessage('error', 'Failed', result.error);
        }
      });
  }

  deleteVideoHomePage(videoHomePage: VideoHomePageDto) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + videoHomePage.youtubeLink + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.videoHomePages = this.videoHomePages.filter((val) => val.id !== videoHomePage.id);

        this.services
          .deleteVideoHomePage(videoHomePage.id)
          .subscribe((result) =>
            result && result.succeeded
              ? this.showMessage('success', 'Successful', 'Attachment Type Deleted')
              : this.showMessage('error', 'Failed', 'Delete Attachment Type Failed')
          );
      },
    });
  }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
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
}
