import { Injectable } from '@angular/core';
import { ShippingAppClients, MemberShip } from 'app/api-clients/shippingapp-client';

@Injectable({
  providedIn: 'root'
})
export class MemberShipService {
  constructor(private client: ShippingAppClients) { }

  getMemberShip() {
    return this.client.getAllMemberShip();
  }

  getMemberShipById(id: string) {
    return this.client.getMemberShipById(id);
  }

  updateMemberShip(id: string, MemberShip: MemberShip) {
    return this.client.updateMemberShip(id, MemberShip);
  }

  deleteMemberShip(id: string) {
    return this.client.deletedMemberShip(id);
  }
}
