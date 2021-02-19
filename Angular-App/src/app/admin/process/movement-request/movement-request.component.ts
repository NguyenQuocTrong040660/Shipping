import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormArray } from '@angular/forms';
import { MenuItem, ConfirmationService } from 'primeng/api';
import { MovementRequestClients, MovementRequestModel, WorkOrderClients, WorkOrderModel } from 'app/shared/api-clients/shipping-app.client';
import { NotificationService } from 'app/shared/services/notification.service';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';

@Component({
  templateUrl: './movement-request.component.html',
  styleUrls: ['./movement-request.component.scss'],
})
export class MovementRequestComponent implements OnInit {
  movementRequests: MovementRequestModel[] = [];
  selectedMovementRequest: MovementRequestModel;
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
  cols: any[] = [];
  colFields = [];
  productsOfSelectedWOs = [];
  TypeColumn = TypeColumn;

  get nameControl() {
    return this.movementRequestForm.get('name');
  }

  get notesControl() {
    return this.movementRequestForm.get('notes');
  }

  get workOrders() {
    return this.movementRequestForm.get('workOrders');
  }

  constructor(
    private workOrderClients: WorkOrderClients,
    private notificationService: NotificationService,
    private movementRequestClients: MovementRequestClients,
    private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Name', field: 'name', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
      { header: '', field: '', width: WidthColumn.IdentityColumn, type: TypeColumn.ExpandColumn }
    ];

    this.colFields = this.cols.map((i) => i.field);


    this.stepItems = [{ label: 'Work Order' }, { label: 'Move Quantity' }, { label: 'Confirm' }];

    this.initMovementRequests();
    this.initWorkOrders();

    this.workOrderItems = [
      {
        id: '1',
        name: 'Product Of WO 1 1',
        workOrderId: 1
      },
      {
        id: '2',
        name: 'Product Of WO 1 2',
        workOrderId: 1
      },
      {
        id: '3',
        name: 'Product Of WO 1 3',
        workOrderId: 1
      },
      {
        id: '4',
        name: 'Product Of WO 2 1',
        workOrderId: 2
      },
      {
        id: '3',
        name: 'Product Of WO 3 1',
        workOrderId: 3
      },
      {
        id: '3',
        name: 'Product Of WO 3 2',
        workOrderId: 3
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
    this.workOrderItems.map(i => {
      if (i.quantity) {
        i.quantity = null;
      }
    });

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
  openDeleteDialog(movememtRequest: MovementRequestModel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.notificationService.success('Delete Movement Request Successfully');
      },
    });
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

  getDetailMovementRequest(movementRequest: MovementRequestModel) {
    // TODO: show Movement Request Detail
  }
}

interface WorkOrderItems {
  id: string;
  name: string;
  workOrderId: number;
  quantity?: number;
}
