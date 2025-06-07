import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentReceiveListComponent } from './payment-receive-list.component';

describe('PaymentReceiveListComponent', () => {
  let component: PaymentReceiveListComponent;
  let fixture: ComponentFixture<PaymentReceiveListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaymentReceiveListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentReceiveListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
