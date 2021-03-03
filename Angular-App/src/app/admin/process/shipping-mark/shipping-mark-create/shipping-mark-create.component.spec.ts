import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingMarkCreateComponent } from './shipping-mark-create.component';

describe('ShippingMarkCreateComponent', () => {
  let component: ShippingMarkCreateComponent;
  let fixture: ComponentFixture<ShippingMarkCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShippingMarkCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShippingMarkCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
