import { Injectable } from '@angular/core';
import { ShippingAppClients, Promotion } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root'
})
export class PromotionService {
  constructor(private client: ShippingAppClients) { }

  getPromotion() {
    return this.client.getAllPromotion();
  }

  getPromotionById(id: string) {
    return this.client.getPromotionById(id);
  }

  updatePromotion(id: string, Promotion: Promotion) {
    return this.client.updatePromotion(id, Promotion);
  }

  deletePromotion(id: string) {
    return this.client.deletedPromotion(id);
  }
}
