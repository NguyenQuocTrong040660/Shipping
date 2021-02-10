import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  templateUrl: './shipping-plan.component.html',
  styleUrls: ['./shipping-plan.component.scss'],
})
export class ShippingPlanComponent implements OnInit {
  shippingPlans: ShippingPlan[] = [];
  selectedShippingPlans: ShippingPlan[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedShippingPlan: ShippingPlan[] = [];
  isDeleteMany: boolean;
  shippingPlanForm: FormGroup;

  get name() {
    return this.shippingPlanForm.get('name');
  }

  ngOnInit() {
    this.shippingPlans = [
      {
        id: '1',
        name: 'Shipping Plan A',
        note: 'This is Shipping Plan A note',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      },
      {
        id: '2',
        name: 'Shipping Plan B',
        note: 'This is Shipping Plan B note',
        created: new Date(),
        createBy: 'Mr.B',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.B 1'
      },
      {
        id: '3',
        name: 'Shipping Plan C',
        note: 'This is Shipping Plan C note',
        created: new Date(),
        createBy: 'Mr.C',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.C 1'
      },
      {
        id: '4',
        name: 'Shipping Plan D',
        note: 'This is Shipping Plan D note',
        created: new Date(),
        createBy: 'Mr.D',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.D 1'
      },
      {
        id: '5',
        name: 'Shipping Plan E',
        note: 'This is Shipping Plan E note',
        created: new Date(),
        createBy: 'Mr.E',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.E 1'
      },
      {
        id: '6',
        name: 'Shipping Plan F',
        note: 'This is Shipping Plan F note',
        created: new Date(),
        createBy: 'Mr.F',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.F 1'
      },
    ];

    this.shippingPlanForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
      created: new FormControl(new Date()),
      createBy: new FormControl(''),
      lastModified: new FormControl(new Date()),
      lastModifiedBy: new FormControl(''),
    });
  }

  // Create Shipping Plan
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.shippingPlanForm.reset();
  }

  onCreate() {
    console.log(this.shippingPlanForm.value);

    // this.hideCreateDialog();
  }

  // Edit Shipping Plan
  openEditDialog(shippingPlan: ShippingPlan) {
    this.isShowEditDialog = true;

    this.shippingPlanForm.get('name').setValue(shippingPlan && shippingPlan.name);
    this.shippingPlanForm.get('note').setValue(shippingPlan && shippingPlan.note);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.shippingPlanForm.reset();
  }

  onEdit() {
    console.log(this.shippingPlanForm.value);

    this.hideEditDialog();
  }

  // Delete Shipping Plan
  openDeleteDialog(singleShippingPlan?: ShippingPlan) {
    this.isShowDeleteDialog = true;
    this.currentSelectedShippingPlan = [];

    if (singleShippingPlan) {
      this.isDeleteMany = false;
      this.currentSelectedShippingPlan.push(singleShippingPlan);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedShippingPlans: ' + this.selectedShippingPlans);
    } else {
      console.log('this.currentSelectedShippingPlan: ' + this.currentSelectedShippingPlan);
    }

    this.hideDeleteDialog();
  }
}

interface ShippingPlan {
  id: string;
  name: string;
  note: string;
  created: Date;
  createBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
