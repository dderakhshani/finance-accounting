import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoadianExcelImportDialogComponent } from './moadian-excel-import-dialog.component';

describe('MoadianExcelImportDialogComponent', () => {
  let component: MoadianExcelImportDialogComponent;
  let fixture: ComponentFixture<MoadianExcelImportDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoadianExcelImportDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoadianExcelImportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
