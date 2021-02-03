import { Component, OnInit, HostListener } from '@angular/core';
import { AlbumService } from 'app/core/services/album.service';
import { MessageService } from 'primeng/api';
import { UserService } from 'app/services/user.service';
import { StateService, States } from 'app/services/state.service';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
  providers: [MessageService],
})
export class AdminComponent implements OnInit {
  isShow = false;
  topPosToStartShowing = 200;

  constructor(
    private service: AlbumService,
    private messageService: MessageService,
    private userService: UserService,
    private stateService: StateService,
    private router: Router
  ) {}

  ngOnInit() {
    
    const accessToken = this.stateService.select('accessToken');
    if (accessToken) {
      this.userService.userInfo().subscribe((res) => {
        this.showMessageLoginSuccess();
      });
    } else {
      this.router.navigateByUrl('/login');
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
