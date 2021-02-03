import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'admin-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  return = '';

  get userName() {
    return this.loginForm.get('userName');
  }
  get password() {
    return this.loginForm.get('password');
  }

  constructor(private fb: FormBuilder, private router: Router, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => (this.return = params['return'] || '/news'));
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  login() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    if (this.userName.value == 'Admin@gmail.com' && this.password.value == 'Admin@1234') {
      localStorage.setItem('USERLOGGED', JSON.stringify(this.loginForm.value));
      // Add Service here to verify
      this.router.navigateByUrl(this.return);
    } else {
    }
  }
}
