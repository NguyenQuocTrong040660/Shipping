import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { ConfigurationComponent } from './configuration.component';
import { configurationRoutes } from './configuration.routes';

@NgModule({
  declarations: [ConfigurationComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(configurationRoutes)],
  exports: [],
})
export class ConfigurationModule {}
