import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountHeadToAccountReferenceReportComponent } from './account-head-to-account-reference-report.component';

describe('AccountHeadToAccountReferenceReportComponent', () => {
  let component: AccountHeadToAccountReferenceReportComponent;
  let fixture: ComponentFixture<AccountHeadToAccountReferenceReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountHeadToAccountReferenceReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountHeadToAccountReferenceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
