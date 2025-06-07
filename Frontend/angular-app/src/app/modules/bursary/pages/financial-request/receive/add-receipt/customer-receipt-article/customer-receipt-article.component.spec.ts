import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerReceiptArticleComponent } from './customer-receipt-article.component';

describe('CustomerReceiptArticleComponent', () => {
  let component: CustomerReceiptArticleComponent;
  let fixture: ComponentFixture<CustomerReceiptArticleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerReceiptArticleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerReceiptArticleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
