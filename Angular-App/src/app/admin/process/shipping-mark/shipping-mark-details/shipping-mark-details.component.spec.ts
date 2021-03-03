import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingMarkDetailsComponent } from './shipping-mark-details.component';

describe('ShippingMarkDetailsComponent', () => {
  let component: ShippingMarkDetailsComponent;
  let fixture: ComponentFixture<ShippingMarkDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingMarkDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingMarkDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
