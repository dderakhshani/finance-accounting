import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SSRSReportDialogComponent } from './ssrsreport-dialog.component';

describe('SSRSReportDialogComponent', () => {
  let component: SSRSReportDialogComponent;
  let fixture: ComponentFixture<SSRSReportDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SSRSReportDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SSRSReportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
