import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { NotificationSeverity } from '../enumerations/notification-severity';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(private messageService: MessageService) {}

  error(detail: string, summary = 'Error', timeLife = 3000) {
    this.messageService.add({ severity: NotificationSeverity.error, summary: summary, detail: detail, life: timeLife });
  }

  success(detail: string, summary = 'Success', timeLife = 3000) {
    this.messageService.add({
      severity: NotificationSeverity.success,
      summary: summary,
      detail: detail,
      life: timeLife,
    });
  }
}
