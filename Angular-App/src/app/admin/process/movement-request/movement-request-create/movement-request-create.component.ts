import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MovementRequestDetailModel, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-movement-request-create',
  templateUrl: './movement-request-create.component.html',
  styleUrls: ['./movement-request-create.component.scss'],
})
export class MovementRequestCreateComponent implements OnInit {
  @Input() movementRequestForm: FormGroup;
  @Input() titleDialog = '';
  @Input() workOrders: WorkOrderModel[] = [];
  @Input() isShowDialog = false;
  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  selectedWorkOrders: WorkOrderModel[] = [];
  movementRequestDetails: MovementRequestDetailModel[] = [];
  clonedMovementRequestDetailModels: { [s: string]: MovementRequestDetailModel } = {};

  stepItems: MenuItem[];
  stepIndex = 0;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  constructor() {}

  ngOnInit(): void {}

  hideDialog() {
    this.selectedWorkOrders = [];
    this.movementRequestDetails = [];
    this.stepIndex = 0;
    this.movementRequestForm.reset();
    this.hideDialogEvent.emit();
  }

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        break;
      }
      case 1: {
        break;
      }
    }

    this.stepIndex += 1;
  }

  prevPage() {
    this.stepIndex -= 1;
  }
}
