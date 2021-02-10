import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './shipping-mark.component.html',
  styleUrls: ['./shipping-mark.component.scss']
})
export class ShippingMarkComponent implements OnInit {
  shippingMarks: ShippingMark[] = [];
  selectedShippingMarks: ShippingMark[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedShippingMark: ShippingMark[] = [];
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
        note: 'This is Shipping Mark A note',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      },
      {
        id: '2',
        name: 'Shipping Mark B',
        note: 'This is Shipping Mark B note',
        created: new Date(),
        createBy: 'Mr.B',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.B 1'
      },
      {
        id: '3',
        name: 'Shipping Mark C',
        note: 'This is Shipping Mark C note',
        created: new Date(),
        createBy: 'Mr.C',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.C 1'
      },
      {
        id: '4',
        name: 'Shipping Mark D',
        note: 'This is Shipping Mark D note',
        created: new Date(),
        createBy: 'Mr.D',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.D 1'
      },
      {
        id: '5',
        name: 'Shipping Mark E',
        note: 'This is Shipping Mark E note',
        created: new Date(),
        createBy: 'Mr.E',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.E 1'
      },
      {
        id: '6',
        name: 'Shipping Mark F',
        note: 'This is Shipping Mark F note',
        created: new Date(),
        createBy: 'Mr.F',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.F 1'
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
  openEditDialog(shippingPlan: ShippingMark) {
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
  openDeleteDialog(singleMovementRequest?: ShippingMark) {
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

interface ShippingMark {
  id: string;
  name: string;
  note: string;
  created: Date;
  createBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
