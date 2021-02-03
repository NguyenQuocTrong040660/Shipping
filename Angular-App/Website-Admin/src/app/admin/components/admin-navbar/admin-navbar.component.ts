import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StateService, States } from 'app/services/state.service';
@Component({
  selector: 'admin-navbar',
  templateUrl: './admin-navbar.component.html',
})
export class AdminNavbarComponent implements OnInit {
  constructor(private router: Router, private stateService: StateService) {}

  ngOnInit(): void {}

  logout() {
    this.stateService.resetToken();
    this.router.navigateByUrl('/login');
  }
}
