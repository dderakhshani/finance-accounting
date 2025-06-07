import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentArticleComponent } from './payment-article.component';

describe('PaymentArticleComponent', () => {
  let component: PaymentArticleComponent;
  let fixture: ComponentFixture<PaymentArticleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaymentArticleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentArticleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
