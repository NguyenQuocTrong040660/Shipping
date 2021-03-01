import { environment } from 'environments/environment';
import { IdentityResult } from './../shared/api-clients/user.client';
import { takeUntil } from 'rxjs/operators';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NotificationService } from 'app/shared/services/notification.service';
import { AuthenticationService } from 'app/shared/services/authentication.service';
import { ApplicationUser } from 'app/shared/models/application-user';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit, OnDestroy {
  private destroyed$ = new Subject<void>();

  loginForm: FormGroup;
  submitted = false;
  returnUrl = '';

  get userName() {
    return this.loginForm.get('userName');
  }
  get password() {
    return this.loginForm.get('password');
  }

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => (this.returnUrl = params['returnUrl'] || ''));
    this.initForm();
    this.initLogin();
  }

  initForm() {
    this.loginForm = this.fb.group({
      userName: [environment.production ? '' : 'admin', [Validators.required]],
      password: [environment.production ? '' : 'ad@123456', [Validators.required, Validators.minLength(6)]],
    });
  }

  initLogin() {
    this.authenticationService.user$.pipe(takeUntil(this.destroyed$)).subscribe((user: ApplicationUser) => {
      if (this.router.url.includes('login')) {
        const accessToken = this.authenticationService.getAccessToken();
        const refreshToken = this.authenticationService.getRefreshToken();

        if (user && accessToken && refreshToken) {
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
          this.router.navigate([returnUrl]);
        }
      }
    });
  }

  login() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }

    const { userName, password } = this.loginForm.value;

    this.authenticationService.login(userName, password, true).subscribe(
      (result: IdentityResult) => {
        if (result && result.succeeded) {
          this.notificationService.success('Login successfully');
          this.router.navigateByUrl(this.returnUrl);
        } else {
          this.notificationService.error(result.errorMessage);
        }
      },
      (_) => this.notificationService.error('Please try again')
    );
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
