import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
// import { Result } from 'app/core/clients/user/client';
import { Result, LoginRequest } from 'app/api-clients/user-client';
import { AuthorizeService } from 'app/core/services/authorize.service';
import { UserService } from 'app/services/user.service';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.scss'],
})
export class ForgetPasswordComponent implements OnInit {
  submitted = false;
  forgetPasswordForm: FormGroup;
  private subscription: Subscription;

  get userName() {
    return this.forgetPasswordForm.get('userName');
  }

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private service: UserService
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.forgetPasswordForm = this.fb.group({
      userName: ['', [Validators.required, Validators.email]],
    });
  }

  submitForgetPassword() {
    this.submitted = true;
    console.log('submitForgetPassword');
  }

  goBackLogin() {
    this.router.navigate(['login'], {
      queryParams: { returnUrl: this.router.routerState.snapshot.url },
    });
  }
}
