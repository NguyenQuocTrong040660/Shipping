import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
})
export class ForgetPasswordComponent implements OnInit {
  submitted = false;
  forgetPasswordForm: FormGroup;

  get userName() {
    return this.forgetPasswordForm.get('userName');
  }

  constructor(private fb: FormBuilder, private router: Router) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.forgetPasswordForm = this.fb.group({
      userName: ['', [Validators.required]],
    });
  }

  submitForgetPassword() {
    this.submitted = true;
  }

  goBackLogin() {
    this.router.navigate(['login'], {
      queryParams: { returnUrl: this.router.routerState.snapshot.url },
    });
  }
}
