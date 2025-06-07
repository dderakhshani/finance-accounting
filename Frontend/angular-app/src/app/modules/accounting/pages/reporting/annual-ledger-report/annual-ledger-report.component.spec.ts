import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualLedgerReportComponent } from './annual-ledger-report.component';

describe('AnnualLedgerReportComponent', () => {
  let component: AnnualLedgerReportComponent;
  let fixture: ComponentFixture<AnnualLedgerReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnnualLedgerReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualLedgerReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
