import { CreateUserRequest, CreateUserResult } from './../../../shared/api-clients/user.client';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { RoleModel, UserClient, UserResult } from 'app/shared/api-clients/user.client';
import { tap } from 'rxjs/operators';
import { SelectItem } from 'primeng/api';

@Component({
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
})
export class UserManagementComponent implements OnInit {
  users: UserResult[] = [];
  roles: RoleModel[] = [];

  selectItems: SelectItem[] = [];

  selectedUsers: UserResult[] = [];
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

  constructor(private userClient: UserClient) {}

  ngOnInit() {
    this.initUsers();
    this.initRoles();
    this.initCreateUserForm();
    this.initSetNewPasswordForm();
  }

  initUsers() {
    this.userClient.apiUserAdminUsersGet().subscribe(
      (users) => (this.users = users),
      (_) => (this.users = [])
    );
  }

  initRoles() {
    this.userClient.apiUserAdminRoles().subscribe(
      (roles) => {
        this.roles = roles;
        this.selectItems = this._mapRoleModelToSelectItem(roles);
      },
      (_) => (this.roles = [])
    );
  }

  initSetNewPasswordForm() {
    this.setNewPasswordForm = new FormGroup({
      newPasswordForms: new FormArray([]),
    });
  }

  initCreateUserForm() {
    this.createUserForm = new FormGroup({
      userForms: new FormArray([this.userFormsInit()]),
    });

    this.userForms.valueChanges.subscribe((i) => console.log(i));
  }

  _mapRoleModelToSelectItem(roles: RoleModel[]): SelectItem[] {
    return roles.map((role) => ({
      value: role.id,
      label: role.name,
    }));
  }

  getControlByNameAndIndex(name: string, index: number) {
    return this.userForms.controls[index].get(name);
  }

  handleOnChangeDropdown(selectedValue: any, name: string, index: number) {
    const roleControl = this.userForms.controls[index].get(name);
    const { value } = selectedValue;
    roleControl.setValue(value.id);
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
    return new FormGroup(
      {
        email: new FormControl('', [Validators.required, Validators.email]),
        userName: new FormControl('', [Validators.required]),
        roleId: new FormControl('', [Validators.required]),
      },
      this.matchContent('email', 'userName')
    );
  }

  setUserName(index: number) {
    const email = this.userForms.controls[index].get('email').value;
    this.userForms.controls[index].get('userName').setValue(email);
  }

  resetCreateUserForm() {
    this.userForms.clear();
    this.addUser();
  }

  onCreate() {
    if (this.createUserForm.valid) {
      const { userForms } = this.createUserForm.value;

      const createUserRequets: CreateUserResult[] = [];

      userForms.forEach((item) => {
        const { userName, email, roleId } = item;

        const request: CreateUserRequest = {
          userName,
          email,
          roleId,
        };

        createUserRequets.push(request);
      });

      this.createUserForm.disable();
      this.userClient.apiUserAdminUsersPost(createUserRequets).subscribe(
        (i) => {
          console.log(i);

          this.hideCreateDialog();
          this.initUsers();
        },
        (_) => this.hideCreateDialog()
      );
    }

    // this.hideCreateDialog();
  }

  openSetNewPasswordDialog() {
    this.isShowSetNewPassworDialog = true;

    this.selectedUsers.forEach((u) => {
      this.newPasswordForms.push(this.newPasswordFormsInit(u.email));
    });
  }

  hideSetNewPasswordDialog() {
    this.isShowSetNewPassworDialog = false;
    this.resetSetNewPasswordForm();
  }

  newPasswordFormsInit(email: string): FormGroup {
    return new FormGroup(
      {
        email: new FormControl(email, [Validators.required, Validators.email]),
        confirmEmail: new FormControl('', [Validators.required, Validators.email]),
      },
      this.matchContent('email', 'confirmEmail')
    );
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
