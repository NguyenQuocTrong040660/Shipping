import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { YoutubeEmbedManagementsRoutingModule } from './youtube-embed-managements-routing.module';
import { YoutubeEmbedManagementsComponent } from './container/youtube-embed-managements.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [YoutubeEmbedManagementsComponent],
  imports: [CommonModule, YoutubeEmbedManagementsRoutingModule, SharedModule],
})
export class YoutubeEmbedManagementsModule {}
