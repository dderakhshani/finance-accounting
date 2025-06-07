import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportChequeComponent } from './report-cheque.component';

describe('ReportChequeComponent', () => {
  let component: ReportChequeComponent;
  let fixture: ComponentFixture<ReportChequeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportChequeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportChequeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
