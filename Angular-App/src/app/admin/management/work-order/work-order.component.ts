import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  templateUrl: './work-order.component.html',
  styleUrls: ['./work-order.component.scss'],
})
export class WorkOrderComponent implements OnInit {
  workOrders: WorkOrder[] = [];
  selectedWorkOrders: WorkOrder[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedWorkOrder: WorkOrder[] = [];
  isDeleteMany: boolean;
  workOrderForm: FormGroup;

  get name() {
    return this.workOrderForm.get('name');
  }

  ngOnInit() {
    this.workOrders = [
      {
        id: '1',
        name: 'Work Order A',
        note: 'This is Work Order A note',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1',
      },
      {
        id: '2',
        name: 'Work Order B',
        note: 'This is Work Order B note',
        created: new Date(),
        createBy: 'Mr.B',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.B 1',
      },
      {
        id: '3',
        name: 'Work Order C',
        note: 'This is Work Order C note',
        created: new Date(),
        createBy: 'Mr.C',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.C 1',
      },
      {
        id: '4',
        name: 'Work Order D',
        note: 'This is Work Order D note',
        created: new Date(),
        createBy: 'Mr.D',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.D 1',
      },
      {
        id: '5',
        name: 'Work Order E',
        note: 'This is Work Order E note',
        created: new Date(),
        createBy: 'Mr.E',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.E 1',
      },
      {
        id: '6',
        name: 'Work Order F',
        note: 'This is Work Order F note',
        created: new Date(),
        createBy: 'Mr.F',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.F 1',
      },
    ];

    this.workOrderForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
      created: new FormControl(new Date()),
      createBy: new FormControl(''),
      lastModified: new FormControl(new Date()),
      lastModifiedBy: new FormControl(''),
    });
  }

  // Create Work Order
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.workOrderForm.reset();
  }

  onCreate() {
    console.log(this.workOrderForm.value);

    // this.hideCreateDialog();
  }

  // Edit Work Order
  openEditDialog(workOrder: WorkOrder) {
    this.isShowEditDialog = true;

    this.workOrderForm.get('name').setValue(workOrder && workOrder.name);
    this.workOrderForm.get('note').setValue(workOrder && workOrder.note);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.workOrderForm.reset();
  }

  onEdit() {
    console.log(this.workOrderForm.value);

    this.hideEditDialog();
  }

  // Delete Work Order
  openDeleteDialog(singleWorkOrder?: WorkOrder) {
    this.isShowDeleteDialog = true;
    this.currentSelectedWorkOrder = [];

    if (singleWorkOrder) {
      this.isDeleteMany = false;
      this.currentSelectedWorkOrder.push(singleWorkOrder);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedWorkOrders: ' + this.selectedWorkOrders);
    } else {
      console.log('this.currentSelectedWorkOrder: ' + this.currentSelectedWorkOrder);
    }

    this.hideDeleteDialog();
  }
}

interface WorkOrder {
  id: string;
  name: string;
  note: string;
  created: Date;
  createBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
