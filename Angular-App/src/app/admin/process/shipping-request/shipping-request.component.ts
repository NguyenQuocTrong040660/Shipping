import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './shipping-request.component.html',
  styleUrls: ['./shipping-request.component.scss']
})
export class ShippingRequestComponent implements OnInit {
  shippingRequests: ShippingRequest[] = [];
  selectedShippingRequests: ShippingRequest[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedShippingRequest: ShippingRequest[] = [];
  isDeleteMany: boolean;
  shippingRequestForm: FormGroup;

  get name() {
    return this.shippingRequestForm.get('name');
  }

  ngOnInit() {
    this.shippingRequests = [
      {
        id: '1',
        name: 'Shipping Request A',
        note: 'This is Shipping Request A note',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      },
      {
        id: '2',
        name: 'Shipping Request B',
        note: 'This is Shipping Request B note',
        created: new Date(),
        createBy: 'Mr.B',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.B 1'
      },
      {
        id: '3',
        name: 'Shipping Request C',
        note: 'This is Shipping Request C note',
        created: new Date(),
        createBy: 'Mr.C',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.C 1'
      },
      {
        id: '4',
        name: 'Shipping Request D',
        note: 'This is Shipping Request D note',
        created: new Date(),
        createBy: 'Mr.D',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.D 1'
      },
      {
        id: '5',
        name: 'Shipping Request E',
        note: 'This is Shipping Request E note',
        created: new Date(),
        createBy: 'Mr.E',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.E 1'
      },
      {
        id: '6',
        name: 'Shipping Request F',
        note: 'This is Shipping Request F note',
        created: new Date(),
        createBy: 'Mr.F',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.F 1'
      }
    ];

    this.shippingRequestForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
    });
  }

  // Create Shipping Request
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.shippingRequestForm.reset();
  }

  onCreate() {
    console.log(this.shippingRequestForm.value);

    // this.hideCreateDialog();
  }

  // Edit Shipping Request
  openEditDialog(shippingPlan: ShippingRequest) {
    this.isShowEditDialog = true;

    this.shippingRequestForm.get('name').setValue(shippingPlan && shippingPlan.name);
    this.shippingRequestForm.get('note').setValue(shippingPlan && shippingPlan.note);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.shippingRequestForm.reset();
  }

  onEdit() {
    console.log(this.shippingRequestForm.value);

    this.hideEditDialog();
  }

  // Delete Shipping Request
  openDeleteDialog(singleShippingRequest?: ShippingRequest) {
    this.isShowDeleteDialog = true;
    this.currentSelectedShippingRequest = [];

    if (singleShippingRequest) {
      this.isDeleteMany = false;
      this.currentSelectedShippingRequest.push(singleShippingRequest);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedShippingRequests: ' + this.selectedShippingRequests);
    } else {
      console.log('this.currentSelectedShippingRequest: ' + this.currentSelectedShippingRequest);
    }

    this.hideDeleteDialog();
  }
}

interface ShippingRequest {
  id: string;
  name: string;
  note: string;
  created: Date;
  createBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
