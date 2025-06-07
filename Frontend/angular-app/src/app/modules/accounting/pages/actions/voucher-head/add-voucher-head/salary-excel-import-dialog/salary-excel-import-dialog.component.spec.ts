import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryExcelImportDialogComponent } from './salary-excel-import-dialog.component';

describe('SalaryExcelImportDialogComponent', () => {
  let component: SalaryExcelImportDialogComponent;
  let fixture: ComponentFixture<SalaryExcelImportDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalaryExcelImportDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryExcelImportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
