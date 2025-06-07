import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerReceiptAttachmentComponent } from './customer-receipt-attachment.component';

describe('CustomerReceiptAttachmentComponent', () => {
  let component: CustomerReceiptAttachmentComponent;
  let fixture: ComponentFixture<CustomerReceiptAttachmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerReceiptAttachmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerReceiptAttachmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
