import { Router } from '@angular/router';
import { ChangePasswordRequest } from './../../../shared/api-clients/user.client';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { UserClient } from 'app/shared/api-clients/user.client';
import { ApplicationUser } from 'app/shared/models/application-user';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { NotificationService } from 'app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
})
export class UserProfileComponent implements OnInit, OnDestroy {
  user: ApplicationUser;

  changePasswordForm: FormGroup;
  submitted = false;
  returnUrl = '';

  get oldPassword() {
    return this.changePasswordForm.get('oldPassword');
  }

  get newPassword() {
    return this.changePasswordForm.get('newPassword');
  }

  get confirmPassword() {
    return this.changePasswordForm.get('confirmPassword');
  }

  private destroyed$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private router: Router,
    private userClient: UserClient
  ) {}

  ngOnInit(): void {
    this.initForm();

    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => (this.user = user));
  }

  initForm() {
    this.changePasswordForm = this.fb.group(
      {
        oldPassword: ['', [Validators.required, Validators.minLength(6)]],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
      },
      { validators: [this.checkConfirmPasswords] }
    );
  }

  changePassword() {
    this.submitted = true;

    if (this.changePasswordForm.invalid) {
      return;
    }

    const { oldPassword, newPassword, confirmPassword } = this.changePasswordForm.value;

    const request: ChangePasswordRequest = {
      newPassword: newPassword,
      confirmPassword: confirmPassword,
      oldPassword: oldPassword,
    };

    this.userClient.apiUserProfileChangePassword(request).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Change password successfully. Please re-login again');
          setTimeout(() => this.authenticationService.logout(), 1000);
        } else {
          this.notificationService.error(result?.error);
        }
      },
      (_) => this.notificationService.error('Change password failed. Please try again')
    );
  }

  checkConfirmPasswords(formGroup: FormGroup) {
    if (formGroup) {
      const password = formGroup.get('newPassword').value as string;
      const confirmPassword = formGroup.get('confirmPassword').value as string;

      if (confirmPassword.length > 5) {
        return password === confirmPassword
          ? null
          : formGroup.get('confirmPassword').setErrors({ invalidConfirmPassword: 'Confirm password does not match with your new password' });
      }
    }

    return;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
