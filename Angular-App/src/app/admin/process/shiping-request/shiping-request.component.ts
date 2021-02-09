import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './shiping-request.component.html',
  styleUrls: ['./shiping-request.component.scss']
})
export class ShippingRequestComponent implements OnInit {
  shippingRequests: { id: string, name: string, note: string }[] = [];
  selectedShippingRequests: { id: string; name: string; note: string }[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedShippingRequest: { id: string; name: string; note: string }[] = [];
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
        note: 'This is Shipping Request A note'
      },
      {
        id: '2',
        name: 'Shipping Request B',
        note: 'This is Shipping Request B note'
      },
      {
        id: '3',
        name: 'Shipping Request C',
        note: 'This is Shipping Request C note'
      },
      {
        id: '4',
        name: 'Shipping Request D',
        note: 'This is Shipping Request D note'
      },
      {
        id: '5',
        name: 'Shipping Request E',
        note: 'This is Shipping Request E note'
      },
      {
        id: '6',
        name: 'Shipping Request F',
        note: 'This is Shipping Request F note'
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
  openEditDialog(shippingPlan: { id: string; name: string; note: string }) {
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
  openDeleteDialog(singleShippingRequest?: { id: string; name: string; note: string }) {
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
