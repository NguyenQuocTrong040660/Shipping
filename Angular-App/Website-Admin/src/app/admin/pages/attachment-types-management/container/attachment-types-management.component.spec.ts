import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AttachmentTypesManagementComponent } from './attachment-types-management.component';

describe('AttachmentTypesManagementComponent', () => {
  let component: AttachmentTypesManagementComponent;
  let fixture: ComponentFixture<AttachmentTypesManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AttachmentTypesManagementComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AttachmentTypesManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
