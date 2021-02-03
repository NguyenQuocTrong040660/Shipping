import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ProgressBarModule } from 'primeng/progressbar';
import { LoadingService } from './core/services/loading.service';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserAnimationsModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ProgressBarModule,
  ],
  providers: [LoadingService],
  bootstrap: [AppComponent],
})
export class AppModule {}
