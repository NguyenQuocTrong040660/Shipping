import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserClient } from './user.client';
import { FilesClient } from './files.client';
import {
  ConfigClients,
  CountryClients,
  EmailNotificationClients,
  HealthClients,
  MovementRequestClients,
  ProductClients,
  ReceivedMarkClients,
  ShippingMarkClients,
  ShippingPlanClients,
  ShippingRequestClients,
  WorkOrderClients,
} from './shipping-app.client';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    UserClient,
    FilesClient,
    ConfigClients,
    CountryClients,
    EmailNotificationClients,
    HealthClients,
    MovementRequestClients,
    ProductClients,
    ReceivedMarkClients,
    ShippingMarkClients,
    ShippingPlanClients,
    ShippingRequestClients,
    WorkOrderClients,
  ],
})
export class ApiClientModule {}
