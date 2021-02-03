import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizeService } from 'app/core/services/authorize.service';
import { UserService } from 'app/services/user.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  providers: [MessageService],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;
  return = '';

  get email() {
    return this.registerForm.get('email');
  }
  get password() {
    return this.registerForm.get('password');
  }
  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  constructor(
    private authorizeService: AuthorizeService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => (this.return = params['returnUrl']));
    this.initForm();
  }

  initForm() {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
    });
  }

  register() {
    this.submitted = true;

    if (this.registerForm.invalid) {
      return;
    }

    this.userService.register(this.registerForm.value).subscribe((res) => {
      if (res && res.succeeded) {
        this.showMessage('success', 'Đăng ký tài khoản thành công');
        this.router.navigateByUrl('/login?registersuccess=true');
        this.router.navigate(['login'], { queryParams: { id: 'register' } });
      } else {
        this.showMessage('error', 'Đăng ký thất bại', res?.error);
      }
    });
  }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
  }
  goBackLogin() {
    this.router.navigate(['login'], {
      queryParams: { returnUrl: this.router.routerState.snapshot.url },
    });
  }
}
