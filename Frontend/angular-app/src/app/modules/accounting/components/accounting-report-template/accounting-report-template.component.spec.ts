import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountingReportTemplateComponent } from './accounting-report-template.component';

describe('AccountingReportTemplateComponent', () => {
  let component: AccountingReportTemplateComponent;
  let fixture: ComponentFixture<AccountingReportTemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountingReportTemplateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountingReportTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
