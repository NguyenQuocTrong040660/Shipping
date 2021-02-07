import { Component, OnInit } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
})
export class UserComponent implements OnInit {
  users: { id: string; userName: string; email: string }[] = [];
  selectedUsers: { id: string; userName: string; email: string }[] = [];
  isShowCreateDialog: boolean;
  createUserForm: FormGroup;

  ngOnInit() {
    this.users = [
      {
        id: '1',
        userName: 'UserA@gmail.com',
        email: 'UserA@gmail.com',
      },
      {
        id: '2',
        userName: 'UserB@gmail.com',
        email: 'UserB@gmail.com',
      },
      {
        id: '3',
        userName: 'UserC@gmail.com',
        email: 'UserC@gmail.com',
      },
      {
        id: '4',
        userName: 'UserD@gmail.com',
        email: 'UserD@gmail.com',
      },
      {
        id: '5',
        userName: 'UserE@gmail.com',
        email: 'UserE@gmail.com',
      },
      {
        id: '6',
        userName: 'UserF@gmail.com',
        email: 'UserF@gmail.com',
      },
    ];

    this.createUserForm = new FormGroup({
      users: new FormArray([]),
    });
  }

  openCreateDialog() {
    this.isShowCreateDialog = true;
  }
  openSetNewPasswordDialog() {}
  openDeleteDialog() {}

  hideDialog() {
    this.isShowCreateDialog = false;
  }

  onCreate() {
    this.isShowCreateDialog = false;
  }
  onSetNewPassword() {}
  onDelete() {}
}
