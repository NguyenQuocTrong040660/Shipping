import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { YoutubeEmbedManagementsComponent } from './container/youtube-embed-managements.component';

const routes: Routes = [
  {
    path: '',
    component: YoutubeEmbedManagementsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class YoutubeEmbedManagementsRoutingModule {}
