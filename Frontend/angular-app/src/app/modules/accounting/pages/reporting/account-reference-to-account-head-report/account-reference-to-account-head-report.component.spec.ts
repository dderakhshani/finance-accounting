import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountReferenceToAccountHeadReportComponent } from './account-reference-to-account-head-report.component';

describe('AccountReferenceToAccountHeadReportComponent', () => {
  let component: AccountReferenceToAccountHeadReportComponent;
  let fixture: ComponentFixture<AccountReferenceToAccountHeadReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountReferenceToAccountHeadReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountReferenceToAccountHeadReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
