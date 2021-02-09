import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './movement-request.component.html',
  styleUrls: ['./movement-request.component.scss']
})
export class MovementRequestComponent implements OnInit {
  movementRequests: { id: string, name: string, note: string }[] = [];
  selectedMovementRequests: { id: string; name: string; note: string }[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedMovementRequest: { id: string; name: string; note: string }[] = [];
  isDeleteMany: boolean;
  movementRequestForm: FormGroup;

  get name() {
    return this.movementRequestForm.get('name');
  }

  ngOnInit() {
    this.movementRequests = [
      {
        id: '1',
        name: 'Movement Request A',
        note: 'This is Movement Request A note'
      },
      {
        id: '2',
        name: 'Movement Request B',
        note: 'This is Movement Request B note'
      },
      {
        id: '3',
        name: 'Movement Request C',
        note: 'This is Movement Request C note'
      },
      {
        id: '4',
        name: 'Movement Request D',
        note: 'This is Movement Request D note'
      },
      {
        id: '5',
        name: 'Movement Request E',
        note: 'This is Movement Request E note'
      },
      {
        id: '6',
        name: 'Movement Request F',
        note: 'This is Movement Request F note'
      }
    ];

    this.movementRequestForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
    });
  }

  // Create Movement Request
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.movementRequestForm.reset();
  }

  onCreate() {
    console.log(this.movementRequestForm.value);

    // this.hideCreateDialog();
  }

  // Edit Movement Request
  openEditDialog(shippingPlan: { id: string; name: string; note: string }) {
    this.isShowEditDialog = true;

    this.movementRequestForm.get('name').setValue(shippingPlan && shippingPlan.name);
    this.movementRequestForm.get('note').setValue(shippingPlan && shippingPlan.note);
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
  openDeleteDialog(singleMovementRequest?: { id: string; name: string; note: string }) {
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
}
