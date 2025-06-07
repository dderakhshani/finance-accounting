import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExcelImportDialogComponent } from './excel-import-dialog.component';

describe('ExcelImportDialogComponent', () => {
  let component: ExcelImportDialogComponent;
  let fixture: ComponentFixture<ExcelImportDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExcelImportDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExcelImportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
