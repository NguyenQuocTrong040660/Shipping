import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './shipping-mark.component.html',
  styleUrls: ['./shipping-mark.component.scss']
})
export class ShippingMarkComponent implements OnInit {
  shippingMarks: { id: string, name: string, note: string }[] = [];
  selectedShippingMarks: { id: string; name: string; note: string }[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedShippingMark: { id: string; name: string; note: string }[] = [];
  isDeleteMany: boolean;
  shippingMarkForm: FormGroup;

  get name() {
    return this.shippingMarkForm.get('name');
  }

  ngOnInit() {
    this.shippingMarks = [
      {
        id: '1',
        name: 'Shipping Mark A',
        note: 'This is Shipping Mark A note'
      },
      {
        id: '2',
        name: 'Shipping Mark B',
        note: 'This is Shipping Mark B note'
      },
      {
        id: '3',
        name: 'Shipping Mark C',
        note: 'This is Shipping Mark C note'
      },
      {
        id: '4',
        name: 'Shipping Mark D',
        note: 'This is Shipping Mark D note'
      },
      {
        id: '5',
        name: 'Shipping Mark E',
        note: 'This is Shipping Mark E note'
      },
      {
        id: '6',
        name: 'Shipping Mark F',
        note: 'This is Shipping Mark F note'
      }
    ];

    this.shippingMarkForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
    });
  }

  // Create Shipping Marks
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.shippingMarkForm.reset();
  }

  onCreate() {
    console.log(this.shippingMarkForm.value);

    // this.hideCreateDialog();
  }

  // Edit Shipping Mark
  openEditDialog(shippingPlan: { id: string; name: string; note: string }) {
    this.isShowEditDialog = true;

    this.shippingMarkForm.get('name').setValue(shippingPlan && shippingPlan.name);
    this.shippingMarkForm.get('note').setValue(shippingPlan && shippingPlan.note);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.shippingMarkForm.reset();
  }

  onEdit() {
    console.log(this.shippingMarkForm.value);

    this.hideEditDialog();
  }

  // Delete Shipping Marks
  openDeleteDialog(singleMovementRequest?: { id: string; name: string; note: string }) {
    this.isShowDeleteDialog = true;
    this.currentSelectedShippingMark = [];

    if (singleMovementRequest) {
      this.isDeleteMany = false;
      this.currentSelectedShippingMark.push(singleMovementRequest);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedShippingMarks: ' + this.selectedShippingMarks);
    } else {
      console.log('this.currentSelectedShippingMark: ' + this.currentSelectedShippingMark);
    }

    this.hideDeleteDialog();
  }
}
