import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfitAndLossReportComponent } from './profit-and-loss-report.component';

describe('ProfitAndLossReportComponent', () => {
  let component: ProfitAndLossReportComponent;
  let fixture: ComponentFixture<ProfitAndLossReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfitAndLossReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfitAndLossReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
