import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountHeadReportComponent } from './account-head-report.component';

describe('AccountHeadReportComponent', () => {
  let component: AccountHeadReportComponent;
  let fixture: ComponentFixture<AccountHeadReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountHeadReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountHeadReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
