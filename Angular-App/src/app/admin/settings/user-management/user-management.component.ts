import { NotificationService } from 'app/shared/services/notification.service';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { CreateUserRequest, LockRequest, RoleModel, UserClient, UserResult } from 'app/shared/api-clients/user.client';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';
import { switchMap, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { CommunicationClient } from 'app/shared/api-clients/communications.client';

@Component({
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
})
export class UserManagementComponent implements OnInit {
  title = 'User Management';

  users: CreateUserModel[] = [];
  newUsers: CreateUserModel[] = [];
  selectRoleItems: SelectItem[] = [];
  roles: RoleModel[] = [];

  clonedNewUsers: { [s: string]: CreateUserModel } = {};

  selectedUsers: CreateUserModel[] = [];
  isShowCreateDialog: boolean;
  isShowSetNewPassworDialog: boolean;

  setNewPasswordForm: FormGroup;

  cols: any[] = [];
  fields: any[] = [];

  TypeColumn = TypeColumn;
  emailPattern = '^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:.[a-zA-Z0-9-]+)*$';
  errorUsers: CreateUserModel[] = [];
  hasDuplicateUsers: boolean;

  get newPasswordForms() {
    return this.setNewPasswordForm.get('newPasswordForms') as FormArray;
  }

  private destroyed$ = new Subject<void>();

  constructor(
    private userClient: UserClient,
    private communicationClient: CommunicationClient,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Email', field: 'email', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'User Name', field: 'userName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Role Name', field: 'roleName', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Locked', field: 'lockoutEnabled', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];
    this.fields = this.cols.map((i) => i.field);

    this.initUsers();
    this.initRoles();
    this.initSetNewPasswordForm();
  }

  initUsers() {
    this.userClient
      .apiUserAdminUsersGet()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (users) => (this.users = users),
        (_) => (this.users = [])
      );
  }

  initRoles() {
    this.userClient
      .apiUserAdminRoles()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (roles) => {
          this.roles = roles;
          this.selectRoleItems = this._mapRoleModelToSelectItem(roles);
        },
        (_) => (this.roles = [])
      );
  }

  initSetNewPasswordForm() {
    this.setNewPasswordForm = new FormGroup({
      newPasswordForms: new FormArray([]),
    });
  }

  handleOnInputEmail(item: CreateUserModel) {
    item.userName = item.email.split('@')[0];
  }

  _mapRoleModelToSelectItem(roles: RoleModel[]): SelectItem[] {
    return roles.map((role) => ({
      value: role.name,
      label: role.name,
    }));
  }

  openCreateDialog() {
    this.isShowCreateDialog = true;
  }

  hideCreateDialog() {
    this.newUsers = [];
    this.errorUsers = [];
    this.isShowCreateDialog = false;
  }

  addUser() {
    const user: CreateUserModel = {
      id: this.createUUID(),
      roleName: this.roles && this.roles.length > 0 ? this.roles[0].name : '',
    };

    this.newUsers = [...this.newUsers, user];
  }

  onCreate() {
    const createUserRequets: CreateUserRequest[] = [];

    this.newUsers.forEach((item) => {
      const request: CreateUserRequest = {
        userName: item.userName,
        email: item.email,
        roleId: this._mapRoleNameToRoleId(item.roleName),
      };

      createUserRequets.push(request);
    });

    this.userClient
      .apiUserAdminUsersPost(createUserRequets)
      .pipe(takeUntil(this.destroyed$))
      .pipe(switchMap((users) => this.communicationClient.apiCommunicationEmailnotificationUsers(users)))
      .subscribe(
        (_) => {
          this.notificationService.success('Create Users Successfully');
          this.hideCreateDialog();
          this.initUsers();
        },
        (_) => {
          this.hideCreateDialog();
          this.initUsers();
        }
      );
  }

  openSetNewPasswordDialog() {
    this.isShowSetNewPassworDialog = true;

    this.selectedUsers.forEach((u, index) => {
      this.newPasswordForms.push(this.newPasswordFormsInit(u.email, index));
    });
  }

  hideSetNewPasswordDialog() {
    this.isShowSetNewPassworDialog = false;
    this.resetSetNewPasswordForm();
  }

  newPasswordFormsInit(email: string, index: number): FormGroup {
    return new FormGroup({
      email: new FormControl(email, [Validators.required, Validators.email]),
      confirmEmail: new FormControl('', [Validators.required, this._matchEmailContent(index)]),
    });
  }

  resetSetNewPasswordForm() {
    this.newPasswordForms.clear();
  }

  onSetNewPassword() {
    const userEmails = [];

    this.newPasswordForms.value.forEach((item) => {
      userEmails.push(item['email']);
    });

    this.userClient
      .apiUserAdminUsersResetPassword(userEmails)
      .pipe(takeUntil(this.destroyed$))
      .pipe(switchMap((users) => this.communicationClient.apiCommunicationEmailnotificationForgotPassword(users)))
      .subscribe(
        (_) => {
          this.notificationService.success('Reset Users Password Successfully');
          this.hideSetNewPasswordDialog();
          this.initUsers();
        },
        (_) => {
          this.hideSetNewPasswordDialog();
          this.notificationService.error('Reset Users Password Failed. Please Try Again');
        }
      );
  }

  openLockDialog() {
    this.confirmationService.confirm({
      message: `Do you want to lock ${this.selectedUsers.length > 0 ? 'these users' : 'this user'} ?`,
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.selectedUsers.forEach((u) => {
          const lockRequest: LockRequest = {
            userId: u.id,
          };
          this.userClient
            .apiUserAdminUserLock(lockRequest)
            .pipe(takeUntil(this.destroyed$))
            .subscribe(
              (result) => {
                if (result && result.succeeded) {
                  this.notificationService.success('Lock User Successfully');
                  this.initUsers();
                } else {
                  this.notificationService.error(result?.error);
                }
              },
              (_) => this.notificationService.error('Lock User Failed. Please Try Again')
            );
        });
      },
    });
  }

  openUnlockDialog() {
    this.confirmationService.confirm({
      message: `Do you want to ${this.selectedUsers.length > 0 ? 'these users' : 'this user'}?`,
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.selectedUsers.forEach((u) => {
          const lockRequest: LockRequest = {
            userId: u.id,
          };
          this.userClient
            .apiUserAdminUserUnlock(lockRequest)
            .pipe(takeUntil(this.destroyed$))
            .subscribe(
              (result) => {
                if (result && result.succeeded) {
                  this.notificationService.success('Unlock User Successfully');
                  this.initUsers();
                } else {
                  this.notificationService.error(result?.error);
                }
              },
              (_) => this.notificationService.error('Unlock User Failed. Please Try Again')
            );
        });
      },
    });
  }

  createUUID(): string {
    let dt = new Date().getTime();
    const uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      const r = (dt + Math.random() * 16) % 16 || 0;
      dt = Math.floor(dt / 16);
      return (c === 'x' ? r : (r && 0x3) || 0x8).toString(16);
    });

    return uuid;
  }

  onRowEditInit(user: CreateUserModel): void {
    user.isEditRow = true;
    this.clonedNewUsers[user.id] = { ...user };
  }

  onRowDelete(user: CreateUserModel): void {
    this.newUsers = this.newUsers.filter((i) => i.id !== user.id);

    this._spliceErrorUser(user);
    this._checkDuplicateEmail();
  }

  async onRowEditSave(user: CreateUserModel) {
    if (!user.email && !user.userName) {
      this.notificationService.error('Please specify a email to register new user');
      delete this.clonedNewUsers[user.id];
      this._spliceErrorUser(user);
      this._checkDuplicateEmail();

      return;
    }

    user.isEditRow = false;
    await this._validateEmail(user);
    this._checkDuplicateEmail();

    delete this.clonedNewUsers[user.id];
  }

  onRowEditCancel(user: CreateUserModel, index: number): void {
    this.newUsers[index] = this.clonedNewUsers[user.id];
    this.newUsers[index].isEditRow = false;
    this._checkDuplicateEmail();
    delete this.clonedNewUsers[user.id];
  }

  allowCreateUsers(newsUsers: CreateUserModel[]): boolean {
    const haveValidEmailRows = newsUsers.every((d) => d.isValidEmail === true && d.hasExistedEmail === false);
    const haveNotEditRows = newsUsers.every((d) => d.isEditRow === false);

    return haveValidEmailRows && haveNotEditRows && newsUsers.length > 0 && !this.hasDuplicateUsers;
  }

  _matchEmailContent(index: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (this.newPasswordForms && this.newPasswordForms.value && this.newPasswordForms.value.length > 0) {
        if (!control.value) {
          return null;
        }

        if (control.value && control.value.length > 0) {
          const email = this.newPasswordForms.controls[index].get('email');
          if (control.value !== email.value) {
            return { matchContent: true };
          }
        }
      }
    };
  }

  _mapRoleNameToRoleId(roleName: string): string {
    return this.roles.find((x) => x.name.toLowerCase() === roleName.toLowerCase()).id;
  }

  async _validateEmail(user: CreateUserModel) {
    const emailRegex = new RegExp(this.emailPattern);

    if (user && emailRegex.test(user.email)) {
      user.isValidEmail = true;

      const result = await this._verifyUserEmail(user);
      if (result && result.error) {
        user.hasExistedEmail = true;

        if (!this.errorUsers.find((u) => u.id === user.id)) {
          this.errorUsers.push(user);
        }
      } else {
        user.hasExistedEmail = false;
        this._spliceErrorUser(user);
      }
    } else {
      user.isValidEmail = false;

      const result = await this._verifyUserEmail(user);
      if (result && result.error) {
        user.hasExistedEmail = true;
      } else {
        user.hasExistedEmail = false;
      }
      if (!this.errorUsers.find((u) => u.id === user.id)) {
        this.errorUsers.push(user);
      }
    }
  }

  _checkDuplicateEmail() {
    if (this.newUsers && this.newUsers.length > 1) {
      const userNames = this.newUsers.map((u) => u.userName);

      this.hasDuplicateUsers = new Set(userNames).size !== userNames.length ? true : false;
    } else {
      this.hasDuplicateUsers = false;
    }
  }

  _spliceErrorUser(user: CreateUserModel) {
    if (this.errorUsers && this.errorUsers.length > 0) {
      const userIndexExisted = this.errorUsers.findIndex((u) => u.id === user.id);
      if (userIndexExisted > -1) {
        this.errorUsers.splice(userIndexExisted, 1);
      }
    }
  }

  async _verifyUserEmail(user: CreateUserModel) {
    return await this.userClient.apiUserAdminUserVerify(user.email).pipe(takeUntil(this.destroyed$)).toPromise();
  }
}

export interface CreateUserModel extends UserResult {
  isEditRow?: boolean;
  isValidEmail?: boolean;
  isDuplicateEmail?: boolean;
  hasExistedEmail?: boolean;
}
