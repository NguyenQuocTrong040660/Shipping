import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './movement-request.component.html',
  styleUrls: ['./movement-request.component.scss']
})
export class MovementRequestComponent implements OnInit {
  movementRequests: MovementRequest[] = [];
  selectedMovementRequests: MovementRequest[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedMovementRequest: MovementRequest[] = [];
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
        note: 'This is Movement Request A note',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      },
      {
        id: '2',
        name: 'Movement Request B',
        note: 'This is Movement Request B note',
        created: new Date(),
        createBy: 'Mr.B',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.B 1'
      },
      {
        id: '3',
        name: 'Movement Request C',
        note: 'This is Movement Request C note',
        created: new Date(),
        createBy: 'Mr.C',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.C 1'
      },
      {
        id: '4',
        name: 'Movement Request D',
        note: 'This is Movement Request D note',
        created: new Date(),
        createBy: 'Mr.D',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.D 1'
      },
      {
        id: '5',
        name: 'Movement Request E',
        note: 'This is Movement Request E note',
        created: new Date(),
        createBy: 'Mr.E',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.E 1'
      },
      {
        id: '6',
        name: 'Movement Request F',
        note: 'This is Movement Request F note',
        created: new Date(),
        createBy: 'Mr.F',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.F 1'
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
  openEditDialog(shippingPlan: MovementRequest) {
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
  openDeleteDialog(singleMovementRequest?: MovementRequest) {
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

interface MovementRequest {
  id: string;
  name: string;
  note: string;
  created: Date;
  createBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
