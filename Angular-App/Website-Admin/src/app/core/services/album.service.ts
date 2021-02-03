import { Injectable } from '@angular/core';
import { AlbumClient, AttachmentTypeDto, FileParameter } from 'app/api-clients/album-client';

@Injectable({
  providedIn: 'root',
})
export class AlbumService {
  constructor(private client: AlbumClient) {}

  healthCheckServer() {
    return this.client.apiHealth();
  }

  getAllPhotoAttachments() {
    return this.client.apiAttachmentsPhoto();
  }

  getAllVideoAttachments() {
    return this.client.apiAttachmentsVideo();
  }

  postAttachment(file: FileParameter, attachmentTypeId: string) {
    return this.client.apiAttachmentsPost(file, attachmentTypeId);
  }

  deleteAttachment(id: string) {
    return this.client.apiAttachmentsDelete(id);
  }

  getAllAttachmentTypes() {
    return this.client.apiAttachmenttypeGet();
  }

  getAttachmentTypeById(id: string) {
    return this.client.apiAttachmenttypeDetail(id);
  }

  addAttachmentType(model: AttachmentTypeDto) {
    return this.client.apiAttachmenttypePost(model);
  }

  updateAttachmentType(id: string, model: AttachmentTypeDto) {
    return this.client.apiAttachmenttypePut(id, model);
  }

  deleteAttachmentType(id: string) {
    return this.client.apiAttachmenttypeDelete(id);
  }

  getAllVideoHomePages() {
    return this.client.apiVideohomepageGet();
  }

  addVideoHomePage(
    id: string,
    width: number,
    height: number,
    youtubeLink: string,
    youtubeId: string,
    youtubeImage: string,
    descriptions: string,
    code: string,
    file: FileParameter
  ) {
    return this.client.apiVideohomepagePost(
      id,
      width,
      height,
      youtubeLink,
      youtubeId,
      youtubeImage,
      descriptions,
      code,
      null,
      null,
      file
    );
  }

  updateVideoHomePage(
    key: string,
    id: string,
    width: number,
    height: number,
    youtubeLink: string,
    youtubeId: string,
    youtubeImage: string,
    descriptions: string,
    code: string,
    file: FileParameter
  ) {
    return this.client.apiVideohomepagePut(
      key,
      id,
      width,
      height,
      youtubeLink,
      youtubeId,
      youtubeImage,
      descriptions,
      code,
      null,
      null,
      file
    );
  }

  deleteVideoHomePage(id: string) {
    return this.client.apiVideohomepageDelete(id);
  }

  deleteAttachments(items: string[]) {
    return this.client.apiAttachmentsBulkdeleteattachments(items);
  }
}
