import { Injectable } from '@angular/core';
import { ShippingAppClients, Country } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root',
})
export class CountryService {
  constructor(private client: ShippingAppClients) {}

  getAllCountry() {
    return this.client.getAllCountry();
  }

  getCountryById(id: string) {
    return this.client.getCountryById(id);
  }

  updateCountry(id: string, country: Country) {
    return this.client.updateCountry(id, country);
  }

  deleteCountry(id: string) {
    return this.client.deleteCountry(id);
  }

  addCountry(country: Country) {
    return this.client.addCountry(country);
  }
}
