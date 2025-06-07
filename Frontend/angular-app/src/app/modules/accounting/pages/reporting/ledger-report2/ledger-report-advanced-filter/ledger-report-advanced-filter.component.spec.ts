import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LedgerReportAdvancedFilterComponent } from './ledger-report-advanced-filter.component';

describe('LedgerReportAdvancedFilterComponent', () => {
  let component: LedgerReportAdvancedFilterComponent;
  let fixture: ComponentFixture<LedgerReportAdvancedFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LedgerReportAdvancedFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LedgerReportAdvancedFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
