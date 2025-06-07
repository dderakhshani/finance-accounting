import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseCountReportComponent } from './warehouse-count-report.component';

describe('WarehouseCountReportComponent', () => {
  let component: WarehouseCountReportComponent;
  let fixture: ComponentFixture<WarehouseCountReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseCountReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseCountReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
