import { Injectable } from '@angular/core';
import { ShippingAppClients, Reservation } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private client: ShippingAppClients) { }

  getcategories() {
    return this.client.categories();
  }

  getProducts() {
    return this.client.getAllProducts(0);
  }

  getProductTypes() {
    return this.client.getAllProductType(0);
  }

  getAllCountry() {
    return this.client.getProductCountries();
  }

  getBrand() {
    return this.client.getProductBrand();
  }
}
