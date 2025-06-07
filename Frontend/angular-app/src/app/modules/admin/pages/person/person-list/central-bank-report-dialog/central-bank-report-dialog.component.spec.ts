import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CentralBankReportDialogComponent } from './central-bank-report-dialog.component';

describe('CentralBankReportDialogComponent', () => {
  let component: CentralBankReportDialogComponent;
  let fixture: ComponentFixture<CentralBankReportDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CentralBankReportDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CentralBankReportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
