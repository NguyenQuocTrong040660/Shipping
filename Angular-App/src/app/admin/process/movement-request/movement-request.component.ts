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
  productsOfSelectedWOs = [];

  get name() {
    return this.movementRequestForm.get('name');
  }

  get workOrders() {
    return this.movementRequestForm.get('workOrders');
  }

  constructor(private workOrderClients: WorkOrderClients, private notificationService: NotificationService, private movementRequestClients: MovementRequestClients) { }

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
        name: 'Product Of WO 1 1',
        workOrderId: 1,
      },
      {
        id: '2',
        name: 'Product Of WO 1 2',
        workOrderId: 1,
      },
      {
        id: '3',
        name: 'Product Of WO 1 3',
        workOrderId: 1,
      },
      {
        id: '4',
        name: 'Product Of WO 2 1',
        workOrderId: 2,
      },
      {
        id: '3',
        name: 'Product Of WO 3 1',
        workOrderId: 3,
      },
      {
        id: '3',
        name: 'Product Of WO 3 2',
        workOrderId: 3,
      },
    ];

    this.initForm();
  }

  initWorkOrders() {
    // this.workOrderClients.getWorkOrders().subscribe(
    //   (i) => (this.workOrderList = i),
    //   (_) => (this.workOrderList = [])
    // );

    this.workOrderList = [
      {
        id: 1,
        productNumber: 'Work Order 1',
      },
      {
        id: 2,
        productNumber: 'Work Order 2',
      },
      {
        id: 3,
        productNumber: 'Work Order 3',
      },
      {
        id: 4,
        productNumber: 'Work Order 4',
      },
      {
        id: 5,
        productNumber: 'Work Order 5',
      },
      {
        id: 6,
        productNumber: 'Work Order 6',
      },
    ];
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
    console.log(this.productsOfSelectedWOs);

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

  nextPage(currentIndex: number) {
    switch (currentIndex) {
      case 0: {
        this.productsOfSelectedWOs = [];
        this.workOrders.value.forEach(wo => {
          const a = this.workOrderItems.filter(i => i.workOrderId === wo.id).forEach(i => {
            this.productsOfSelectedWOs.push(i);
          });
        });

        break;
      }
      case 1: {
        break;
      }
      case 2: {

      }
    }

    this.activeIndex += 1;
  }

  prevPage() {
    this.activeIndex -= 1;
  }

  removeProduct(product: WorkOrderItems) {
    this.productsOfSelectedWOs = this.productsOfSelectedWOs.filter(p => p.id !== product.id);
  }

  isDisableStep2(): boolean {
    return this.productsOfSelectedWOs.some(p => !p.quantity);
  }
}

interface WorkOrderItems {
  id: string;
  name: string;
  workOrderId: number;
}
