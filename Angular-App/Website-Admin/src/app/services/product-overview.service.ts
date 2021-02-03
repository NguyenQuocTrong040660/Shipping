import { Injectable } from '@angular/core';
import { ShippingAppClients, ProductOverview, FileParameter } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root'
})
export class ProductOverviewService {

  constructor(private client: ShippingAppClients) { }

  insertProductOverview(productOverview: ProductOverview) {
    return this.client.addProducts(productOverview);
  }
  getAllProductOverview(companyIndex) {
    return this.client.getAllProducts(companyIndex);
  }

  deletedProductOverView(productID) {
    return this.client.deletedProduct(productID);
  }
  uploadImg(file: FileParameter) {
    return this.client.uploadImage(file);
  }

  getProductOverViewById(id) {
    return this.client.getProductsbyID(id);
  }
  updateProductOverView(id: string, product: ProductOverview) {
    return this.client.updateProduct(id, product);
  }

}
