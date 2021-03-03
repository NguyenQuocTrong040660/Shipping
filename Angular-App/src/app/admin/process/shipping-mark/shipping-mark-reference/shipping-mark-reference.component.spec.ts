import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingMarkReferenceComponent } from './shipping-mark-reference.component';

describe('ShippingMarkReferenceComponent', () => {
  let component: ShippingMarkReferenceComponent;
  let fixture: ComponentFixture<ShippingMarkReferenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingMarkReferenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingMarkReferenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
