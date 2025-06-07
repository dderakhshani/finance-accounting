import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountReviewReportComponent } from './account-review-report.component';

describe('AccountReviewReportComponent', () => {
  let component: AccountReviewReportComponent;
  let fixture: ComponentFixture<AccountReviewReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountReviewReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountReviewReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
