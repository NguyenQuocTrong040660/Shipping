import { AfterContentChecked, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { LoadingService } from './shared/services/loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, AfterContentChecked {
  loading = false;

  constructor(private loadingService: LoadingService, private router: Router, private cdr: ChangeDetectorRef) {
    this.router.events.subscribe((event) => {
      switch (true) {
        case event instanceof NavigationStart: {
          this.loading = true;
          break;
        }

        case event instanceof NavigationEnd:
        case event instanceof NavigationCancel:
        case event instanceof NavigationError: {
          this.loading = false;
          break;
        }
        default: {
          break;
        }
      }
    });
  }

  ngOnInit() {
    this.loadingService.IsLoading.subscribe((i) => (this.loading = i));
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
  }
}
