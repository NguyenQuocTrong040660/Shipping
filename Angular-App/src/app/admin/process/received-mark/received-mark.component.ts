import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './received-mark.component.html',
  styleUrls: ['./received-mark.component.scss']
})
export class ReceivedMarkComponent implements OnInit {
  receivedMarks: ReceivedMark[] = [];
  selectedReceivedMarks: ReceivedMark[] = [];
  isShowCreateDialog: boolean;
  isShowEditDialog: boolean;
  isShowDeleteDialog: boolean;
  currentSelectedReceivedMark: ReceivedMark[] = [];
  isDeleteMany: boolean;
  receivedMarkForm: FormGroup;

  get name() {
    return this.receivedMarkForm.get('name');
  }

  ngOnInit() {
    this.receivedMarks = [
      {
        id: '1',
        name: 'Received Mark A',
        note: 'This is Received Mark A note',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      },
      {
        id: '2',
        name: 'Received Mark B',
        note: 'This is Received Mark B note',
        created: new Date(),
        createBy: 'Mr.B',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.B 1'
      },
      {
        id: '3',
        name: 'Received Mark C',
        note: 'This is Received Mark C note',
        created: new Date(),
        createBy: 'Mr.C',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.C 1'
      },
      {
        id: '4',
        name: 'Received Mark D',
        note: 'This is Received Mark D note',
        created: new Date(),
        createBy: 'Mr.D',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.D 1'
      },
      {
        id: '5',
        name: 'Received Mark E',
        note: 'This is Received Mark E note',
        created: new Date(),
        createBy: 'Mr.E',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.E 1'
      },
      {
        id: '6',
        name: 'Received Mark F',
        note: 'This is Received Mark F note',
        created: new Date(),
        createBy: 'Mr.F',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.F 1'
      }
    ];

    this.receivedMarkForm = new FormGroup({
      name: new FormControl('', Validators.required),
      note: new FormControl(''),
    });
  }

  // Create Received Marks
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.receivedMarkForm.reset();
  }

  onCreate() {
    console.log(this.receivedMarkForm.value);

    // this.hideCreateDialog();
  }

  // Edit Received Mark
  openEditDialog(shippingPlan: ReceivedMark) {
    this.isShowEditDialog = true;

    this.receivedMarkForm.get('name').setValue(shippingPlan && shippingPlan.name);
    this.receivedMarkForm.get('note').setValue(shippingPlan && shippingPlan.note);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.receivedMarkForm.reset();
  }

  onEdit() {
    console.log(this.receivedMarkForm.value);

    this.hideEditDialog();
  }

  // Delete Received Marks
  openDeleteDialog(singleMovementRequest?: ReceivedMark) {
    this.isShowDeleteDialog = true;
    this.currentSelectedReceivedMark = [];

    if (singleMovementRequest) {
      this.isDeleteMany = false;
      this.currentSelectedReceivedMark.push(singleMovementRequest);
    } else {
      this.isDeleteMany = true;
    }
  }

  hideDeleteDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    if (this.isDeleteMany) {
      console.log('this.selectedReceivedMarks: ' + this.selectedReceivedMarks);
    } else {
      console.log('this.currentSelectedReceivedMark: ' + this.currentSelectedReceivedMark);
    }

    this.hideDeleteDialog();
  }
}

interface ReceivedMark {
  id: string;
  name: string;
  note: string;
  created: Date;
  createBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
