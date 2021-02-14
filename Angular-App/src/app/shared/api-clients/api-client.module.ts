import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { API_BASE_URL as USER_BASE_URL, UserClient } from './user.client';
import { API_BASE_URL as FILES_BASE_URL, FilesClient } from './files.client';
import {
  API_BASE_URL as SHIPPING_BASE_URL,
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
import { environment } from 'environments/environment';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [
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
    { provide: USER_BASE_URL, useValue: environment.baseUrl },
    { provide: SHIPPING_BASE_URL, useValue: environment.baseUrl },
    { provide: FILES_BASE_URL, useValue: environment.baseUrl },
  ],
})
export class ApiClientModule {}
