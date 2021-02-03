import { Injectable } from '@angular/core';
import { ShippingAppClients, Brand } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root',
})
export class BrandService {
  constructor(private client: ShippingAppClients) { }

  getAllBrand(CompanyIndex) {
    return this.client.getAllBrand(CompanyIndex);
  }

  getBrandById(id: string) {
    return this.client.getBrandById(id);
  }

  addBrand(brand: Brand) {
    return this.client.addBrand(brand);
  }

  updateBrand(brandCode: string, brand: Brand) {
    return this.client.updateBrand(brandCode, brand);
  }

  deleteBrand(brandCode: string) {
    return this.client.deleteBrand(brandCode);
  }
}
