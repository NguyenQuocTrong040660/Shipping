import { Injectable } from '@angular/core';
import { ShippingAppClients, Reservation } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  constructor(private client: ShippingAppClients) { }

  getReservation() {
    return this.client.getAllReservation();
  }

  getReservationById(id: string) {
    return this.client.getReservationById(id);
  }

  updateReservation(id: string, reservation: Reservation) {
    return this.client.updateReservation(id, reservation);
  }

  deleteReservation(id: string) {
    return this.client.deletedReservation(id);
  }
}
