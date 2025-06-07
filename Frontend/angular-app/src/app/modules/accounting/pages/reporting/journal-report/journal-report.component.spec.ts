import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalReportComponent } from './journal-report.component';

describe('JournalReportComponent', () => {
  let component: JournalReportComponent;
  let fixture: ComponentFixture<JournalReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JournalReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
