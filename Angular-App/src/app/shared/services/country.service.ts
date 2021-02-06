import { Injectable } from '@angular/core';
import { ShippingAppClients, Country } from 'app/shared/api-clients/shipping-app.client';

@Injectable({
  providedIn: 'root',
})
export class CountryService {
  constructor(private client: ShippingAppClients) {}

  getAllCountry() {
    return this.client.apiShippingappCountryGetallcountry();
  }

  getCountryById(id: string) {
    return this.client.apiShippingappCountryGetcountrybyid(id);
  }

  updateCountry(id: string, country: Country) {
    return this.client.apiShippingappCountryUpdatecountry(id, country);
  }

  deleteCountry(id: string) {
    return this.client.apiShippingappCountryDeletecountry(id);
  }

  addCountry(country: Country) {
    return this.client.apiShippingappCountryAddcountry(country);
  }
}
