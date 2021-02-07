import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-user',
    templateUrl: './user-management.component.html',
    styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
	users: {id: string, userName: string, email: string}[] = [];
	selectedUsers: {id: string, userName: string, email: string}[] = [];
	isShowCreateDialog: boolean;
	createUserForm: FormGroup;
	emailRegex: string = '[a-z0-9!#$%&\'*+\/=?^_`{|}~.-]+@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*';

	get userForms() {
    return this.createUserForm.get('userForms') as FormArray;
	}

	ngOnInit() {
    this.users = [
        {
            id: '1',
            userName: 'UserA@gmail.com',
            email: 'UserA@gmail.com'
        },
        {
            id: '2',
            userName: 'UserB@gmail.com',
            email: 'UserB@gmail.com'
        },
        {
            id: '3',
            userName: 'UserC@gmail.com',
            email: 'UserC@gmail.com'
        },
        {
            id: '4',
            userName: 'UserD@gmail.com',
            email: 'UserD@gmail.com'
        },
        {
            id: '5',
            userName: 'UserE@gmail.com',
            email: 'UserE@gmail.com'
        },
        {
            id: '6',
            userName: 'UserF@gmail.com',
            email: 'UserF@gmail.com'
        }
    ];

    this.createUserForm = new FormGroup({
        userForms: new FormArray([
            this.userFormsInit()
        ])
    });
	}

	openCreateDialog() {
    this.isShowCreateDialog = true;
	}
	openSetNewPasswordDialog() {}
	openDeleteDialog() {}

	hideDialog() {
    this.isShowCreateDialog = false;
    this.resetCreateUserForm();
	}

	onCreate() {
    // this.isShowCreateDialog = false;

    console.log('this.userForms: ', this.createUserForm.value);
	}
	onSetNewPassword() {}
	onDelete() {}

	addUser() {
		this.userForms.push(this.userFormsInit());
	}

	removeUser(index: number) {
		this.userForms.removeAt(index);
	}

	userFormsInit(): FormGroup {
    return new FormGroup({
        email: new FormControl('', [Validators.required, Validators.pattern(this.emailRegex)]),
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

  private matchContent(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (control.value !== matchingControl.value) {
        return { matchContent: true }
      } else {
        return null
      }
    }
  }
}
