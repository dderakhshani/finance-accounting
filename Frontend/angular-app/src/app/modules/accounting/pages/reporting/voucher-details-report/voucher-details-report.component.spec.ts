import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherDetailsReportComponent } from './voucher-details-report.component';

describe('VoucherDetailsReportComponent', () => {
  let component: VoucherDetailsReportComponent;
  let fixture: ComponentFixture<VoucherDetailsReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VoucherDetailsReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherDetailsReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
