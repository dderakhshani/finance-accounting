import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRequestPaymentComponent } from './add-request-payment.component';

describe('AddRequestPaymentComponent', () => {
  let component: AddRequestPaymentComponent;
  let fixture: ComponentFixture<AddRequestPaymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddRequestPaymentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddRequestPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
