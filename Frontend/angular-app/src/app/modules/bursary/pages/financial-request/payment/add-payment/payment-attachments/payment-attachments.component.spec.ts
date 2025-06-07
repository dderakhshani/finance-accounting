import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentAttachmentsComponent } from './payment-attachments.component';

describe('PaymentAttachmentsComponent', () => {
  let component: PaymentAttachmentsComponent;
  let fixture: ComponentFixture<PaymentAttachmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaymentAttachmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentAttachmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
