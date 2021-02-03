import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YoutubeEmbedManagementsComponent } from './youtube-embed-managements.component';

describe('YoutubeEmbedManagementsComponent', () => {
  let component: YoutubeEmbedManagementsComponent;
  let fixture: ComponentFixture<YoutubeEmbedManagementsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [YoutubeEmbedManagementsComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YoutubeEmbedManagementsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
