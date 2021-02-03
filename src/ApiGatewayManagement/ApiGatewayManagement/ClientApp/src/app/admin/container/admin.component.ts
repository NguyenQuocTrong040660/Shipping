import { Component, OnInit, HostListener } from '@angular/core';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
  providers: [MessageService],
})
export class AdminComponent implements OnInit {
  isShow = false;
  topPosToStartShowing = 200;

  constructor(private messageService: MessageService) {}

  ngOnInit() {
    // this.service.healthCheckServer().subscribe(
    //   (_) => {
    //     console.log('Health check');
    //   },
    //   (_) => {
    //     this.messageService.add({
    //       severity: 'error',
    //       summary: 'Sever Error',
    //       detail: 'Could not init connection to server! Please contact system admin',
    //       life: 6000,
    //     });
    //   }
    // );
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
