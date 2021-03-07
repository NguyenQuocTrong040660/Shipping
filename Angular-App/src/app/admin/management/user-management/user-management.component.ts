import { NotificationService } from 'app/shared/services/notification.service';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
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
  title = 'Users Management';

  users: UserResult[] = [];
  newUsers: UserResult[] = [];
  selectRoleItems: SelectItem[] = [];
  roles: RoleModel[] = [];

  clonedNewUsers: { [s: string]: UserResult } = {};

  selectedUsers: UserResult[] = [];
  isShowCreateDialog: boolean;
  isShowSetNewPassworDialog: boolean;

  setNewPasswordForm: FormGroup;

  cols: any[] = [];
  fields: any[] = [];

  TypeColumn = TypeColumn;

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

  handleOnInputEmail(item: UserResult) {
    if (item.email.includes('@')) {
      item.userName = item.email.split('@')[0];
      return;
    }

    item.userName = item.email;
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
    this.isShowCreateDialog = false;
  }

  addUser() {
    const user: UserResult = {
      id: this.createUUID(),
      userName: 'email',
      email: 'email@example.com',
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
          this.hideCreateDialog();
          this.initUsers();
        },
        (_) => {
          this.hideCreateDialog();
          this.notificationService.error('Create Users failed. Please try again');
        }
      );
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
    return new FormGroup({
      email: new FormControl(email, [Validators.required, Validators.email]),
      confirmEmail: new FormControl('', [Validators.required, Validators.email]),
    });
  }

  resetSetNewPasswordForm() {
    this.newPasswordForms.clear();
  }

  onSetNewPassword() {
    console.log('this.setNewPasswordForm: ', this.setNewPasswordForm.value);
  }

  openLockDialog() {
    this.confirmationService.confirm({
      message: `Are you sure you want to lock ${this.selectedUsers.length > 0 ? 'these users' : 'this user'} ?`,
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
              (_) => this.notificationService.error('Lock User Failed. Please try again')
            );
        });
      },
    });
  }

  openUnlockDialog() {
    this.confirmationService.confirm({
      message: `Are you sure you want to ${this.selectedUsers.length > 0 ? 'these users' : 'this user'}?`,
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
              (_) => this.notificationService.error('Unlock User Failed. Please try again')
            );
        });
      },
    });
  }

  createUUID(): string {
    let dt = new Date().getTime();
    let uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      let r = (dt + Math.random() * 16) % 16 | 0;
      dt = Math.floor(dt / 16);
      return (c == 'x' ? r : (r & 0x3) | 0x8).toString(16);
    });

    return uuid;
  }

  onRowEditInit(user: UserResult): void {
    this.clonedNewUsers[user.id] = { ...user };
  }

  onRowDelete(user: UserResult): void {
    this.newUsers = this.newUsers.filter((i) => i.id !== user.id);
  }

  onRowEditSave(user: UserResult): void {
    if (!user.email && !user.userName) {
      this.notificationService.error('Please specify a email to register new user');
    }

    delete this.clonedNewUsers[user.id];
  }

  onRowEditCancel(user: UserResult, index: number): void {
    this.newUsers[index] = this.clonedNewUsers[user.id];
    delete this.clonedNewUsers[user.id];
  }

  canCreateUsers(newsUser: UserResult[]): boolean {
    if (newsUser.length === 0) {
      return false;
    }

    return newsUser.filter((i) => !!i.email === false).length === 0;
  }

  _mapRoleNameToRoleId(roleName: string): string {
    return this.roles.find((x) => x.name.toLowerCase() === roleName.toLowerCase()).id;
  }
}
