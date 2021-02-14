import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormArray } from '@angular/forms';
import { MenuItem } from 'primeng/api';
import { MovementRequestClients, MovementRequestModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { NotificationService } from 'app/shared/services/notification.service';

@Component({
  templateUrl: './movement-request.component.html',
  styleUrls: ['./movement-request.component.scss'],
})
export class MovementRequestComponent implements OnInit {
  movementRequests: MovementRequestModel[] = [];
  selectedMovementRequests: MovementRequestModel[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedMovementRequest: MovementRequestModel[] = [];
  isDeleteMany: boolean;
  movementRequestForm: FormGroup;
  stepItems: MenuItem[] = [];
  activeIndex = 0;
  workOrderList: WorkOrderModel[] = [];
  workOrderItems: WorkOrderItems[] = [];
  cols: { header: string; field: string }[] = [];
  colFields = [];

  get name() {
    return this.movementRequestForm.get('name');
  }

  get workOrders() {
    return this.movementRequestForm.get('workOrders');
  }

  constructor(private workOrderClients: WorkOrderClients, private notificationService: NotificationService, private movementRequestClients: MovementRequestClients) {}

  ngOnInit() {
    this.cols = [
      { header: 'Name', field: 'name' },
      { header: 'Note', field: 'note' },
      { header: 'Created', field: 'created' },
      { header: 'Create By', field: 'createBy' },
      { header: 'Last Modified', field: 'lastModified' },
      { header: 'Last Modified By', field: 'lastModifiedBy' },
    ];
    this.stepItems = [{ label: 'Work Order' }, { label: 'Move Quantity' }, { label: 'Confirm' }];
    this.colFields = this.cols.map((i) => i.field);

    this.initMovementRequests();
    this.initWorkOrders();

    this.workOrderItems = [
      {
        id: '1',
        name: 'Work Order Item 1',
        workOrderId: '1',
      },
      {
        id: '2',
        name: 'Work Order Item 1.1',
        workOrderId: '1',
      },
      {
        id: '3',
        name: 'Work Order Item 1.2',
        workOrderId: '1',
      },
      {
        id: '4',
        name: 'Work Order Item 2',
        workOrderId: '2',
      },
      {
        id: '3',
        name: 'Work Order Item 3',
        workOrderId: '3',
      },
      {
        id: '3',
        name: 'Work Order Item 3.1',
        workOrderId: '3',
      },
    ];

    this.initForm();
  }

  initWorkOrders() {
    this.workOrderClients.getWorkOrders().subscribe(
      (i) => (this.workOrderList = i),
      (_) => (this.workOrderList = [])
    );
  }

  initMovementRequests() {
    this.movementRequestClients.getMovementRequests().subscribe(
      (i) => (this.movementRequests = i),
      (_) => (this.movementRequests = [])
    );
  }

  initForm() {
    this.movementRequestForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
      workOrders: new FormControl([], Validators.required),
    });
  }

  // Create Movement Request
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.activeIndex = 0;
    this.movementRequestForm.reset();
  }

  onCreate() {
    console.log(this.movementRequestForm.value);

    this.hideCreateDialog();
  }

  // Edit Movement Request
  openEditDialog(request: MovementRequestModel) {
    this.isShowEditDialog = true;

    this.movementRequestForm.setValue(request);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.movementRequestForm.reset();
  }

  onEdit() {
    console.log(this.movementRequestForm.value);

    this.hideEditDialog();
  }

  // Delete Movement Request
  openDeleteDialog(singleMovementRequest?: MovementRequestModel) {
    this.isShowDeleteDialog = true;
    this.currentSelectedMovementRequest = [];

    if (singleMovementRequest) {
      this.isDeleteMany = false;
      this.currentSelectedMovementRequest.push(singleMovementRequest);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedShippingRequests: ' + this.selectedMovementRequests);
    } else {
      console.log('this.currentSelectedMovementRequest: ' + this.currentSelectedMovementRequest);
    }

    this.hideDeleteDialog();
  }

  nextPage() {
    this.activeIndex += 1;
  }

  prevPage() {
    this.activeIndex -= 1;
  }
}

interface WorkOrderItems {
  id: string;
  name: string;
  workOrderId: string;
}
