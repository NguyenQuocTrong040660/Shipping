import { ProductOverviewService } from 'app/services/product-overview.service';
import { FileParameter } from 'app/api-clients/album-client';

export class UploadAdapter {
  private loader;
  constructor(loader: any, private service: ProductOverviewService) {
    this.loader = loader;
  }

  uploadFile(file) {
    const fileToUpload: FileParameter = {
      data: file,
      fileName: file.name,
    };

    return this.service.uploadImg(fileToUpload);
  }

  upload() {
    return this.loader.file.then(
      (file) =>
        new Promise((resolve, reject) => {
          this.uploadFile(file).subscribe(
            (result) => {
              resolve({ default: result['imageUrl'] });
            },
            (error) => {
              reject(error);
            }
          );
        })
    );
  }

  // upload() {
  //   return this.loader.file.then(
  //     (file) =>
  //       new Promise((resolve, reject) => {
  //         var myReader = new FileReader();
  //         myReader.onloadend = (e) => {
  //           resolve({ default: myReader.result });
  //         };

  //         myReader.readAsDataURL(file);
  //       })
  //   );
  // }
}
