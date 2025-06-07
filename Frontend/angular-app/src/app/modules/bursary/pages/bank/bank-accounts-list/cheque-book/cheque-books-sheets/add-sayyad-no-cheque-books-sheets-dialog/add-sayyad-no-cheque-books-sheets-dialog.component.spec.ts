import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSayyadNoChequeBooksSheetsDialogComponent } from './add-sayyad-no-cheque-books-sheets-dialog.component';

describe('AddSayyadNoChequeBooksSheetsDialogComponent', () => {
  let component: AddSayyadNoChequeBooksSheetsDialogComponent;
  let fixture: ComponentFixture<AddSayyadNoChequeBooksSheetsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddSayyadNoChequeBooksSheetsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddSayyadNoChequeBooksSheetsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
