import { environment } from 'environments/environment';
import { Component, OnInit, HostListener } from '@angular/core';
import { MessageService } from 'primeng/api';
import { StateService } from 'app/core/services/state.service';
import { Router } from '@angular/router';
import { UserClient } from 'app/api-clients/user-client';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
  isShow = false;
  topPosToStartShowing = 200;

  constructor(
    private messageService: MessageService,
    private userClient: UserClient,
    private stateService: StateService,
    private router: Router
  ) {}

  ngOnInit() {
    if (environment.production) {
      const accessToken = this.stateService.select('accessToken');
      if (accessToken) {
        this.userClient.apiUserUserInfo().subscribe((_) => {
          this.showMessageLoginSuccess();
        });
      } else {
        this.router.navigateByUrl('/login');
      }
    }
  }

  showMessageLoginSuccess() {
    this.messageService.add({
      severity: 'success',
      summary: 'Đăng nhập thành công',
      detail: '',
      life: 3000,
    });
  }

  @HostListener('window:scroll')
  checkScroll() {
    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
    if (scrollPosition >= this.topPosToStartShowing) {
      this.isShow = true;
    } else {
      this.isShow = false;
    }
  }

  gotoTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  }
}
