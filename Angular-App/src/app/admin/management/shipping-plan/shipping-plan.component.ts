import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  templateUrl: './shipping-plan.component.html',
  styleUrls: ['./shipping-plan.component.scss'],
})
export class ShippingPlanComponent implements OnInit {
  shippingPlans: { id: string; name: string; note: string }[] = [];
  selectedShippingPlans: { id: string; name: string; note: string }[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedShippingPlan: { id: string; name: string; note: string }[] = [];
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
      },
      {
        id: '2',
        name: 'Shipping Plan B',
        note: 'This is Shipping Plan B note',
      },
      {
        id: '3',
        name: 'Shipping Plan C',
        note: 'This is Shipping Plan C note',
      },
      {
        id: '4',
        name: 'Shipping Plan D',
        note: 'This is Shipping Plan D note',
      },
      {
        id: '5',
        name: 'Shipping Plan E',
        note: 'This is Shipping Plan E note',
      },
      {
        id: '6',
        name: 'Shipping Plan F',
        note: 'This is Shipping Plan F note',
      },
    ];

    this.shippingPlanForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
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
  openEditDialog(shippingPlan: { id: string; name: string; note: string }) {
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
  openDeleteDialog(singleShippingPlan?: { id: string; name: string; note: string }) {
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
