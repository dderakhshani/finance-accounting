import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QReportComponent } from './qreport.component';

describe('QReportComponent', () => {
  let component: QReportComponent;
  let fixture: ComponentFixture<QReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
