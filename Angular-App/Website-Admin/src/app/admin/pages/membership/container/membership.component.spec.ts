import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberShipComponent } from './membership.component';

describe('MemberShipComponent', () => {
  let component: MemberShipComponent;
  let fixture: ComponentFixture<MemberShipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MemberShipComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberShipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
