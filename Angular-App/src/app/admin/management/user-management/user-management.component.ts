import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  users: { id: string, userName: string, email: string }[] = [];
  selectedUsers: { id: string, userName: string, email: string }[] = [];
  isShowCreateDialog: boolean;
  isShowSetNewPassworDialog: boolean;
  isShowDeleteDialog: boolean;
  createUserForm: FormGroup;
  setNewPasswordForm: FormGroup;

  get userForms() {
    return this.createUserForm.get('userForms') as FormArray;
  }

  get newPasswordForms() {
    return this.setNewPasswordForm.get('newPasswordForms') as FormArray;
  }

  ngOnInit() {
    this.users = [
      {
        id: '1',
        userName: 'user.a@gmail.com',
        email: 'user.a@gmail.com'
      },
      {
        id: '2',
        userName: 'user.b@gmail.com',
        email: 'user.b@gmail.com'
      },
      {
        id: '3',
        userName: 'user.c@gmail.com',
        email: 'user.c@gmail.com'
      },
      {
        id: '4',
        userName: 'user.d@gmail.com',
        email: 'user.d@gmail.com'
      },
      {
        id: '5',
        userName: 'user.e@gmail.com',
        email: 'user.e@gmail.com'
      },
      {
        id: '6',
        userName: 'user.f@gmail.com',
        email: 'user.f@gmail.com'
      }
    ];

    this.createUserForm = new FormGroup({
      userForms: new FormArray([
        this.userFormsInit()
      ])
    });

    this.setNewPasswordForm = new FormGroup({
      newPasswordForms: new FormArray([])
    });
  }

  // Create Users
  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.isShowCreateDialog = false;
    this.resetCreateUserForm();
  }

  addUser() {
    this.userForms.push(this.userFormsInit());
  }

  userFormsInit(): FormGroup {
    return new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      userName: new FormControl('', Validators.required)
    }, this.matchContent('email', 'userName'));
  }

  setUserName(index: number) {
    this.userForms.controls[index].get('userName').setValue(this.userForms.controls[index].value.email);
  }

  resetCreateUserForm() {
    this.userForms.clear();
    this.addUser();
  }

  onCreate() {
    console.log('this.userForms: ', this.createUserForm.value);

    // this.hideCreateDialog();
  }

  // Set New Password
  openSetNewPasswordDialog() {
    this.isShowSetNewPassworDialog = true;

    this.selectedUsers.forEach(u => {
      this.newPasswordForms.push(this.newPasswordFormsInit(u.email));
    });

  }

  hideSetNewPasswordDialog() {
    this.isShowSetNewPassworDialog = false;
    this.resetSetNewPasswordForm();
  }

  newPasswordFormsInit(email: string): FormGroup {
    return new FormGroup({
      email: new FormControl(email, [Validators.required, Validators.email]),
      confirmEmail: new FormControl('', [Validators.required, Validators.email])
    }, this.matchContent('email', 'confirmEmail'));
  }

  resetSetNewPasswordForm() {
    this.newPasswordForms.clear();
  }

  onSetNewPassword() {
    console.log('this.setNewPasswordForm: ', this.setNewPasswordForm.value);

    // this.hideSetNewPasswordDialog();
  }

  // Delete Users
  openDeleteDialog() {
    this.isShowDeleteDialog = true;
  }

  hideDeleteUsersDialog() {
    this.isShowDeleteDialog = false;
  }

  onDelete() {
    console.log('this.selectedUsers: ', this.selectedUsers);

    // this.hideSetNewPasswordDialog();
  }

  // General
  removeUser(index: number, formArray: FormArray) {
    formArray.removeAt(index);
  }

  private matchContent(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (control.value !== matchingControl.value) {
        return { matchContent: true };
      } else {
        return null;
      }
    };
  }
}
