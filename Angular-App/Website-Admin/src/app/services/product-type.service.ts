import { Injectable } from '@angular/core';
import { ShippingAppClients, ProductType } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root'
})
export class ProductTypeService {

  constructor(private client: ShippingAppClients) { }

  postProductType(productType: ProductType) {
    return this.client.productType(productType);
  }
  getProductType(companyIndex) {
    return this.client.getAllProductType(companyIndex);
  }

  getProductTypeById(id) {
    //return this.client.getProductTypeById(id);
  }

  deleteProductType(id) {
    return this.client.deleteProductType(id);
  }

  updateProductType(productTypeCode, productType: ProductType) {
    return this.client.updateProductType(productTypeCode, productType);
  }
}
