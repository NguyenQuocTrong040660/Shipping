import { Component, OnDestroy, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Subject } from 'rxjs';

import { Result, LoginRequest } from 'app/api-clients/user-client';
import { AuthorizeService } from 'app/core/services/authorize.service';
import { UserService } from 'app/services/user.service';
import { MessageService } from 'primeng/api';
import { StateService, States } from 'app/services/state.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  providers: [MessageService],
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm: FormGroup;
  submitted = false;
  loginSuccessMess = [];

  private subscription: Subscription;
  return = '';

  get userName() {
    return this.loginForm.get('userName');
  }
  get password() {
    return this.loginForm.get('password');
  }
  private destroyed$ = new Subject<void>();
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private service: UserService,
    private messageService: MessageService,
    private stateService: StateService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      if (params.id === 'register') {
        this.showMessage('success', 'Đăng ký thành công');
      }
    });
    this.initForm();
  }

  initForm() {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  goToRegister() {
    this.router.navigate(['/register'], {
      queryParams: { returnUrl: this.router.routerState.snapshot.url },
    });
  }

  goToForgetPassword() {
    this.router.navigate(['/forget-password']);
  }

  login() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    const data: LoginRequest = Object.assign({}, this.loginForm.value, {
      rememberMe: false,
    });

    this.service.login(data).subscribe((result) => {
      this.stateService.resetToken();
      if (result && result.succeeded) {
        this.stateService.setState('accessToken', result.accessToken);
        this.stateService.setState('logged', true);
        this.router.navigateByUrl('/');
      } else {
        this.showMessage('error', 'Đăng nhập thất bại', result?.errorMessage);
      }
    });
  }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
